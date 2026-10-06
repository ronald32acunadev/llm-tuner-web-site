# LLM Tuner — Documentation & Showcase Web App

Official informational website and documentation portal for the [`llm-tuner`](https://github.com/ronald32acunadev/llmtuner) npm package, built with **Blazor WebAssembly** on **.NET 10**.

## Features

- **Brand Design System**: Inherits the exact color palette, dark/light theme tokens, and typography (`IBM Plex Sans` & `IBM Plex Mono`) from the core `llmtuner` project.
- **Interactive Visual Showcase**:
  - **Terminal Wizard (`llm-tuner`)**: Interactive terminal mockup with tabs for the step-by-step wizard, headless one-liner, settings menu (`/settings`), and cached presets inspection (`--presets`).
  - **Desktop App (`llm-tuner-desktop`)**: Full Electron GUI mockup featuring hardware telemetry rail, context chips, live benchmark progress table, and real-time chat connection tester.
- **Interactive Command Builder**: Configure engine, model, context tokens, load profiles, and flags with immediate CLI syntax preview and 1-click clipboard copy.
- **Complete CLI Reference**: Categorized, searchable table of all supported flags (`--engine`, `--model`, `--ctx`, `--profile`, `--force`, `--candidates`, `--yes`, `--dry-run`, `--presets`, `--json`, `--lang`, `--theme`, `/settings`, `--help`).
- **Load Profiles Deep Dive**: Interactive matrix comparing `speed`, `balanced`, and `quality` objectives, KV cache constraints, and variant selection strategies.
- **Bilingual (EN / ES)**: Toggle seamlessly between English and Spanish documentation in real time.
- **Theme Switcher**: Dark, Light, and System preferences stored locally in the browser.

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run Locally

```bash
dotnet watch
```

Then navigate to `http://localhost:5000` or the URL provided by the development server.

### Build and Publish

```bash
dotnet publish -c Release -o dist
```

The resulting files in `dist/wwwroot` are static web assets ready to be deployed to GitHub Pages, Cloudflare Pages, Netlify, Vercel, or any static web host.

## License

MIT © Ronald Acuña
