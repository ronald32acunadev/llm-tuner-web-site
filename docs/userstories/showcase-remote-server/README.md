# Showcase: remote server and settings screens

Bring `/showcase` up to llm-tuner 0.5.0: capture the server mode screens and replace the two settings screenshots that predate it.

Requested by the user on 2026-10-29: "Hay que actualizar el showcase tambien". Execution mode: pending.

## Facts (checked in `../../Desktop/llmtuner` 0.5.0)

- `capture.mjs` captures the desktop UI in headless Chrome against an isolated `XDG_CONFIG_HOME`, and the CLI through `script`, interrupting interactive commands after their first screen.
- `/settings` opens a menu with Language, Theme, "Automatic server startup after login", and "View server connection details and token"; autostart status reads only the per-user startup entry, which the isolated config keeps empty.
- `/serve` prints `LLM Tuner server mode running on http://<LAN IP>:<port>` and `Authentication Token: <token>`; it binds the port, so a capture must use a free one.
- The desktop "Local Server Settings" block renders only when Electron's preload exposes `window.llmTunerServer` (`getAutostartStatus`, `setAutostartEnabled`, `revealConnection`, `copyToken`); headless Chrome has no such bridge.
- The desktop "Add Remote Server" dialog opens from `#add-server-btn` in the settings drawer.

## US-01 — Capture the new and outdated screens automatically

As a maintainer, I want the capture script to produce the server screens, so that the showcase stays current without manual captures.

**Acceptance criteria**

1. `capture.mjs` additionally produces:
   - `cli-04-settings.png`: first screen of `llm-tuner /settings` (replaces the manual capture).
   - `cli-07-serve.png`: startup output of `llm-tuner /serve`, run on a free port.
   - `05-settings-drawer.png`: the desktop settings drawer including Servers and "Local Server Settings" (replaces the manual capture).
   - `07-add-server.png`: the desktop "Add Remote Server" dialog filled with demo values (name "Remote GPU Box", URL `http://192.168.1.100:7860`, a token that shows only as masked dots).
2. No real machine data is published: in `cli-07-serve.png` the LAN IP is replaced with `192.168.1.100`, the port with `7860`, and the token with a short demo value before rendering.
3. For the drawer, the script injects a stub `window.llmTunerServer` with demo data before the page loads, so the real UI renders; the stub never touches the user's system, and the token stays masked.
4. The existing guarantees hold: isolated config, free ports, staging, and publishing only when every capture succeeds.

**Dependencies**: none.

**Verification**: run the script; it exits 0 and writes all 9 automatic files; open each new image: dark theme, English, no temp paths, no real IP or token.

**Status**: pending

## US-02 — Showcase sections for the new screens

As a visitor, I want to see the server mode screens explained, so that I recognize them when I set up a remote server.

**Acceptance criteria**

1. `GetShowcaseSections()` gains a CLI section for `cli-07-serve.png` (after "Saved presets") and a desktop section for `07-add-server.png` (after "Settings"), each with an explanation, image, alt text, and command where it applies, in English and Spanish.
2. The CLI "Settings" and desktop "Settings" captions describe the new screenshots and drop the "this screenshot does not show yet" notes.
3. The new sections point to the Remote server guide on `/commands` in one sentence.

**Dependencies**: US-01.

**Verification**: `dotnet build`; `/showcase` shows 15 sections with 15 loaded images, EN/ES, 375 px without horizontal scroll.

**Status**: pending

## US-03 — Keep the skill accurate

As a maintainer, I want the skill to list the new captures, so that future syncs regenerate them.

**Acceptance criteria**

1. `references/screenshots.md` lists the 9 automatic files, the 6 remaining manual ones (desktop `02`–`04`, `cli-01-wizard`, `cli-02-headless`, `cli-03-presets`), and the masking and stub behavior.

**Dependencies**: US-01, US-02.

**Verification**: read the file back.

**Status**: pending
