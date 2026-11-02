# Remote server guide (CLI and desktop)

Add a step-by-step guide to `/commands` that explains how to run llm-tuner as a server on one PC and connect to it from another PC, with the CLI and with the desktop app.

Requested by the user on 2026-10-29: "Falta incluir la informacion de como conectarse a otro pc o levantar el servicio para utilizar el servidor tanto en el cli como en la guid". Execution mode: all stories (approved 2026-10-29).

## Facts (checked in `../../Desktop/llmtuner` 0.5.0)

- `llm-tuner /serve` listens on `0.0.0.0:7860` by default and prints the URL with the PC's LAN IPv4 address plus the token (`src/cli/commands/serve.js`, `resolveConnectableHost` in `src/core/util.js`).
- Token order: `--token`, then the token saved in `server.json`, then a new 32-byte hex token that is saved (`serve.js`).
- Automatic startup after login (CLI `/settings` or desktop Settings) creates `server.json` with `0.0.0.0:7860` and a token if missing, and registers a per-user entry: Linux `~/.config/autostart/llm-tuner-server.desktop`, macOS `~/Library/LaunchAgents`, Windows Startup folder (`src/platform/server-autostart.js`). It takes effect on the next login.
- CLI `/settings` → "View server connection details and token" and desktop Settings → "Local Server Settings" ("Reveal token", "Copy token") show the saved address and token (`src/cli/commands/settings.js`, `src/i18n/*.js`).
- In server mode every request, including the HTML page, requires `Authorization: Bearer <token>`; a browser cannot open the server URL directly (`src/web/server.js`).
- Servers are added only in the desktop app: Settings → Servers → "Add Server" (name, URL, token), "Test Connection", "Save Server"; the saved server becomes active, the list switches it, and "Remove" returns to `local` (`src/web/public/app.js`). There is no CLI command to add a server.
- Desktop and CLI share `settings.json`; the CLI uses the active server for `/load` and `/presets`, `--server <id|name>` changes it and saves the choice, and `--server local` returns to this PC (`src/cli/router.js`, `src/cli/commands/load.js`, `presets.js`).
- Remote requests strip host-identifying hardware details and block `/api/install` with 403; settings, updates, and MCP agent configuration stay local (`server.js`, llm-tuner README).
- Errors: `Unauthorized: a valid Bearer token is required.`, `Forbidden: this operation cannot be performed remotely.`, `Could not connect to server: {error}`, `No server configuration yet. Run /serve or enable automatic startup first.` (`src/i18n/en.js`).
- Not from llm-tuner: the server PC's firewall must allow inbound TCP on the server port (7860 by default). Stated as general network guidance.

## US-01 — Remote server guide in `/commands`

As a user with a GPU PC and another computer, I want step-by-step instructions to start the server and connect to it, so that I can tune and load models remotely from the CLI or the desktop app.

**Acceptance criteria**

1. `/commands` gains a third sidebar group, "Remote server" / "Servidor remoto", after CLI and Desktop, with these sections in order:
   1. How it works: server PC and client PC, what runs remotely and what stays local, trusted-LAN warning.
   2. Start the server (GPU PC): `/serve` with its output, the flags, automatic startup from CLI or desktop and where it is registered per OS, and the firewall note.
   3. Get the address and token: CLI and desktop paths, using the LAN IP (example `http://192.168.1.100:7860`), never `0.0.0.0` or `localhost`.
   4. Connect from the desktop app (client PC): Settings → Servers → "Add Server", "Test Connection", "Save Server", switching, "Remove"; the browser cannot open the server URL directly.
   5. Connect from the CLI (client PC): add the server in the desktop app first, then `--server <name|id>`; the choice is saved and shared with the desktop app; `--server local` to return; applies to `/load` and `/presets`.
   6. Troubleshooting: one entry per error message above with its cause and fix.
2. Every fact matches the Facts list; desktop labels are quoted from `src/i18n/en.js` and `es.js`.
3. English and Spanish; code blocks are full commands.
4. The existing `/serve` and Desktop "Remote servers" sections link the reader to the new group (one sentence each).
5. The `sync-llmtuner` content map lists the new group as the place for remote server changes.

**Dependencies**: none (uses the existing `DocsLayout` and `GetDocSections()`).

**Verification**: `dotnet build`; on `/commands` the sidebar shows three groups and 24 sections, each new link scrolls to its section, EN/ES switch on the page, 375 px without horizontal scroll, console without errors.

**Status**: complete

## Verification results (2026-10-29)

- Content is data in `GetDocSections()` written by the agent from the Facts list; the only markup change is the `remote` group in `Pages/Commands.razor`.
- `dotnet build`: 0 warnings, 0 errors.
- llm-tuner's own `parseCliArgs` parses the documented commands as expected: `--server local /presets` → `{"command":"presets","server":"local"}`; `--server gpu-rig --engine ollama ... --yes` → `load` with `server: "gpu-rig"`; `/serve --port 7860 --token my-secret-token` → `serve` with both values.
- `/commands` at 1280 px: sidebar groups "CLI", "Desktop", "Remote server"; 24 sections and 24 sidebar links; each of the 6 new links places its section at 96 px from the top (the last one at the page end); the `/serve` and Desktop "Remote servers" sections point to the new group.
- Spanish: all 6 headings, the troubleshooting messages, and the cross-references switch with the ES toggle; console without errors.
- 375 px: `scrollWidth` 375 and the "On this page" menu lists the three groups.
- US-01 criterion 5: the `sync-llmtuner` content map lists the remote server sections.
- Note: in the hidden browser pane, smooth scrolling never animates, so anchor checks ran with `scroll-behavior: auto` set from the console only.
