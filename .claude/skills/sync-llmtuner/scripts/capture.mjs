#!/usr/bin/env node
// Captures real screenshots of the llm-tuner desktop app and CLI for the showcase page.
// Usage: node capture.mjs --llmtuner <path-to-llmtuner-repo> --out <screenshots-dir>
// Runs llm-tuner with an isolated XDG_CONFIG_HOME so the user's settings and presets are never touched.
import { spawn } from 'node:child_process';
import fs from 'node:fs/promises';
import os from 'node:os';
import path from 'node:path';

const SERVER_PORT = 7861;
const DEBUG_PORT = 9333;

function parseArgs(argv) {
  const out = {};
  for (let i = 0; i < argv.length; i++) {
    if (argv[i] === '--llmtuner') out.llmtuner = argv[++i];
    else if (argv[i] === '--out') out.out = argv[++i];
  }
  return out;
}

const sleep = (ms) => new Promise((r) => setTimeout(r, ms));

async function poll(fn, timeoutMs, label) {
  const deadline = Date.now() + timeoutMs;
  while (Date.now() < deadline) {
    try {
      const value = await fn();
      if (value) return value;
    } catch { /* not ready yet */ }
    await sleep(200);
  }
  throw new Error(`Timed out waiting for ${label}`);
}

async function findChrome() {
  const candidates = [
    process.env.CHROME_BIN,
    '/usr/bin/google-chrome',
    '/usr/bin/google-chrome-stable',
    '/usr/bin/chromium',
    '/snap/bin/chromium',
    '/usr/bin/chromium-browser',
  ].filter(Boolean);
  for (const bin of candidates) {
    try {
      await fs.access(bin);
      return bin;
    } catch { /* try next */ }
  }
  throw new Error('Chrome or Chromium not found. Set CHROME_BIN.');
}

class Cdp {
  static connect(url) {
    return new Promise((resolve, reject) => {
      const ws = new WebSocket(url);
      ws.addEventListener('open', () => resolve(new Cdp(ws)), { once: true });
      ws.addEventListener('error', () => reject(new Error('CDP connection failed')), { once: true });
    });
  }

  constructor(ws) {
    this.ws = ws;
    this.nextId = 1;
    this.pending = new Map();
    ws.addEventListener('message', (event) => {
      const msg = JSON.parse(event.data);
      if (!msg.id || !this.pending.has(msg.id)) return;
      const { resolve, reject } = this.pending.get(msg.id);
      this.pending.delete(msg.id);
      if (msg.error) reject(new Error(msg.error.message));
      else resolve(msg.result);
    });
  }

  send(method, params = {}) {
    const id = this.nextId++;
    return new Promise((resolve, reject) => {
      this.pending.set(id, { resolve, reject });
      this.ws.send(JSON.stringify({ id, method, params }));
    });
  }

  close() {
    this.ws.close();
  }
}

async function evaluate(cdp, expression) {
  const res = await cdp.send('Runtime.evaluate', { expression, returnByValue: true, awaitPromise: true });
  if (res.exceptionDetails) throw new Error(res.exceptionDetails.text);
  return res.result.value;
}

async function waitFor(cdp, expression, timeoutMs = 30000) {
  const deadline = Date.now() + timeoutMs;
  while (Date.now() < deadline) {
    try {
      if (await evaluate(cdp, expression)) return;
    } catch { /* page still loading */ }
    await sleep(250);
  }
  throw new Error(`Timed out waiting for: ${expression}`);
}

async function setViewport(cdp, width, height, scale) {
  await cdp.send('Emulation.setDeviceMetricsOverride', { width, height, deviceScaleFactor: scale, mobile: false });
  await sleep(300);
}

async function screenshot(cdp, file, { width, height, scale }) {
  await setViewport(cdp, width, height, scale);
  const { data } = await cdp.send('Page.captureScreenshot', { format: 'png', captureBeyondViewport: false });
  await fs.writeFile(file, Buffer.from(data, 'base64'));
  console.log(`captured ${file}`);
}

// ---------- Terminal capture ----------

function runTerminal(llmtuner, args, env, { interruptAfterMs } = {}) {
  const quoted = args.map((a) => `'${a.replaceAll("'", "'\\''")}'`).join(' ');
  const command = `node '${path.join(llmtuner, 'src/cli/index.js')}' ${quoted}`.trim();
  return new Promise((resolve) => {
    const child = spawn('script', ['-qfec', command, '/dev/null'], { env, stdio: ['pipe', 'pipe', 'pipe'] });
    let output = '';
    child.stdout.on('data', (chunk) => { output += chunk.toString('utf8'); });
    const timers = [];
    if (interruptAfterMs) timers.push(setTimeout(() => child.stdin.write('\x03'), interruptAfterMs));
    timers.push(setTimeout(() => child.kill('SIGKILL'), (interruptAfterMs ?? 15000) + 4000));
    child.on('close', () => {
      timers.forEach(clearTimeout);
      resolve(output);
    });
  });
}

const PALETTE = ['#4b5563', '#ff5f56', '#27c93f', '#ffbd2e', '#3b82f6', '#c678dd', '#00e5ff', '#e6edf3'];

const escapeHtml = (s) => s.replaceAll('&', '&amp;').replaceAll('<', '&lt;').replaceAll('>', '&gt;');
const stripAnsi = (s) => s.replace(/\x1b\[[0-9;?]*[A-Za-z]/g, '').replace(/\x1b/g, '');

function applySgr(state, params) {
  const codes = params === '' ? [0] : params.split(';').map(Number);
  for (let i = 0; i < codes.length; i++) {
    const c = codes[i];
    if (c === 0) Object.assign(state, { color: null, bold: false, dim: false, italic: false, underline: false });
    else if (c === 1) state.bold = true;
    else if (c === 2) state.dim = true;
    else if (c === 3) state.italic = true;
    else if (c === 4) state.underline = true;
    else if (c === 22) { state.bold = false; state.dim = false; }
    else if (c === 23) state.italic = false;
    else if (c === 24) state.underline = false;
    else if (c === 39) state.color = null;
    else if (c >= 30 && c <= 37) state.color = PALETTE[c - 30];
    else if (c >= 90 && c <= 97) state.color = PALETTE[c - 90];
    else if (c === 38 || c === 48) {
      const isFg = c === 38;
      if (codes[i + 1] === 5) {
        const n = codes[i + 2];
        if (isFg && n >= 0 && n <= 15) state.color = PALETTE[n % 8];
        i += 2;
      } else if (codes[i + 1] === 2) {
        if (isFg) state.color = `rgb(${codes[i + 2]},${codes[i + 3]},${codes[i + 4]})`;
        i += 4;
      }
    }
    // 40-49 and 100-107 (backgrounds) are ignored.
  }
}

function styleOf(state) {
  const parts = [];
  if (state.color) parts.push(`color:${state.color}`);
  if (state.bold) parts.push('font-weight:700');
  if (state.dim) parts.push('opacity:0.6');
  if (state.italic) parts.push('font-style:italic');
  if (state.underline) parts.push('text-decoration:underline');
  return parts.join(';');
}

function ansiToHtml(text) {
  const clean = text.replace(/\x1b\][^\x07\x1b]*(\x07|\x1b\\)/g, '');
  const state = { color: null, bold: false, dim: false, italic: false, underline: false };
  const lines = [];
  for (let raw of clean.split('\n')) {
    raw = raw.replace(/\r$/, '');
    if (raw.includes('\r')) raw = raw.slice(raw.lastIndexOf('\r') + 1);
    if (stripAnsi(raw).includes('^C')) continue;
    let html = '';
    let i = 0;
    while (i < raw.length) {
      if (raw[i] === '\x1b') {
        const m = /^\x1b\[([0-9;?]*)([A-Za-z])/.exec(raw.slice(i));
        if (m) {
          if (m[2] === 'm') applySgr(state, m[1]);
          i += m[0].length;
        } else {
          i += 1;
        }
        continue;
      }
      let j = i;
      while (j < raw.length && raw[j] !== '\x1b') j++;
      const chunk = escapeHtml(raw.slice(i, j));
      const style = styleOf(state);
      html += style ? `<span style="${style}">${chunk}</span>` : chunk;
      i = j;
    }
    lines.push(html);
  }
  const isBlank = (l) => stripAnsi(l.replace(/<[^>]+>/g, '')).trim() === '';
  while (lines.length && isBlank(lines[0])) lines.shift();
  while (lines.length && isBlank(lines[lines.length - 1])) lines.pop();
  return lines.join('\n');
}

function terminalDocument(body) {
  return `<!doctype html><html><head><meta charset="utf-8"><style>
body{margin:0;background:#090e15}
pre{margin:0;padding:24px 28px;color:#e6edf3;font-family:'Ubuntu Mono','DejaVu Sans Mono',monospace;font-size:14px;line-height:1.45;white-space:pre-wrap;width:760px;box-sizing:border-box}
</style></head><body><pre>${body}</pre></body></html>`;
}

// ---------- Main ----------

async function main() {
  const { llmtuner, out } = parseArgs(process.argv.slice(2));
  if (!llmtuner || !out) {
    console.error('Usage: node capture.mjs --llmtuner <path-to-llmtuner-repo> --out <screenshots-dir>');
    process.exitCode = 1;
    return;
  }

  const tmp = await fs.mkdtemp(path.join(os.tmpdir(), 'llmtuner-capture-'));
  const children = [];
  let cdp;
  try {
    await fs.mkdir(path.join(tmp, 'config'), { recursive: true });
    await fs.mkdir(out, { recursive: true });
    const childEnv = {
      ...process.env,
      XDG_CONFIG_HOME: path.join(tmp, 'config'),
      PORT: String(SERVER_PORT),
      FORCE_COLOR: '1',
      COLUMNS: '80',
      LINES: '40',
    };

    children.push(spawn(process.execPath, [path.join(llmtuner, 'src/web/server.js')], { env: childEnv, stdio: 'ignore' }));
    const serverUrl = `http://127.0.0.1:${SERVER_PORT}/`;
    await poll(async () => (await fetch(serverUrl)).ok, 20000, 'the desktop server');

    const chrome = await findChrome();
    children.push(spawn(chrome, [
      '--headless=new',
      `--remote-debugging-port=${DEBUG_PORT}`,
      `--user-data-dir=${path.join(tmp, 'chrome')}`,
      '--no-first-run',
      '--no-default-browser-check',
      '--hide-scrollbars',
      '--window-size=1200,860',
      'about:blank',
    ], { stdio: 'ignore' }));
    const target = await poll(async () => {
      const list = await (await fetch(`http://127.0.0.1:${DEBUG_PORT}/json/list`)).json();
      return list.find((t) => t.type === 'page');
    }, 20000, 'Chrome DevTools');

    cdp = await Cdp.connect(target.webSocketDebuggerUrl);
    await cdp.send('Page.enable');
    await cdp.send('Runtime.enable');
    // The site is dark-only; match it regardless of the theme the isolated profile resolves to.
    await cdp.send('Emulation.setEmulatedMedia', { features: [{ name: 'prefers-color-scheme', value: 'dark' }] });

    // Desktop app
    const desktop = { width: 1200, height: 860, scale: 2 };
    await setViewport(cdp, desktop.width, desktop.height, desktop.scale);
    await cdp.send('Page.navigate', { url: serverUrl });
    await waitFor(cdp, "document.querySelector('.mcp-btn') !== null");
    await sleep(800);
    await screenshot(cdp, path.join(out, '01-engine-selection.png'), desktop);

    await evaluate(cdp, "document.querySelector('.mcp-btn').click()");
    await waitFor(cdp, `(() => {
      const m = document.querySelector('#mcp-modal');
      return m && !m.hidden && document.querySelectorAll('#mcp-agents-list > *').length > 0;
    })()`);
    await sleep(800);
    await screenshot(cdp, path.join(out, '06-mcp-modal.png'), desktop);

    // CLI
    const cliCaptures = [
      { file: 'cli-01-menu.png', args: [], interruptAfterMs: 4000 },
      { file: 'cli-05-help.png', args: ['--help'] },
      { file: 'cli-06-mcp.png', args: ['/mcp'], interruptAfterMs: 6000 },
    ];
    for (const capture of cliCaptures) {
      const raw = await runTerminal(llmtuner, capture.args, childEnv, capture);
      // Show the standard Linux config path instead of the isolated temp dir.
      const html = terminalDocument(ansiToHtml(raw.replaceAll(childEnv.XDG_CONFIG_HOME, '~/.config')));
      await setViewport(cdp, 760, 400, 2);
      await cdp.send('Page.navigate', { url: 'about:blank' });
      await sleep(300);
      const { frameTree } = await cdp.send('Page.getFrameTree');
      await cdp.send('Page.setDocumentContent', { frameId: frameTree.frame.id, html });
      await sleep(300);
      const height = await evaluate(cdp, "Math.ceil(document.querySelector('pre').getBoundingClientRect().height)");
      await screenshot(cdp, path.join(out, capture.file), { width: 760, height, scale: 2 });
    }
  } finally {
    cdp?.close();
    await Promise.all(children.map((child) => new Promise((resolve) => {
      if (child.exitCode !== null || child.signalCode !== null) return resolve();
      child.once('exit', resolve);
      child.kill('SIGKILL');
    })));
    // Chrome helper processes can still be flushing the profile for a moment after exit.
    await fs.rm(tmp, { recursive: true, force: true, maxRetries: 10, retryDelay: 200 });
  }
}

main().catch((error) => {
  console.error(error.message);
  process.exitCode = 1;
});
