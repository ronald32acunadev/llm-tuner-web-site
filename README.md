# LLM Tuner — Documentation & Showcase Web App

[![Live Web](https://img.shields.io/badge/Firebase-Hosting-FFCA28?logo=firebase&logoColor=black)](https://llm-tuner-b9184.web.app)
[![Version](https://img.shields.io/badge/version-v0.4.2-00e5ff)](https://github.com/ronald32acunadev/llmtuner/releases/tag/v0.4.2)
[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Blazor WebAssembly](https://img.shields.io/badge/Blazor-WebAssembly-512BD4?logo=blazor&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

[English](README.md) · [Español](README.es.md)

Official documentation website and interactive visual portal for the [`llm-tuner`](https://github.com/ronald32acunadev/llmtuner) npm package (v0.4.2), built with **Blazor WebAssembly** on **.NET 10** and deployed to **Firebase Hosting**.

---

## Overview

The web app serves as the central documentation and visual showcase hub for the `llm-tuner` ecosystem. It demonstrates how `llm-tuner` mathematically models GPU VRAM, benchmarks candidate KV cache quantizations, streams real-time hardware telemetry, registers local runtimes as MCP servers for AI agents, and configures local LLM engines (LM Studio and Ollama) with zero guesswork.

The portal provides high-fidelity simulations of both the terminal CLI and Electron desktop interfaces, an interactive command builder with context shorthand support (`8k`, `16k`, `32k`, `64k`), an exhaustive parameters reference, profile comparison matrices, and system architecture breakdowns.

---

## Key Features

- **Independent Routed Views**: Dedicated pages for each core area (`/` or `/overview`, `/what`, `/showcase`, `/commands`, `/profiles`, `/architecture`).
- **Documentation-Style Pages**: `/commands`, `/showcase`, and `/profiles` share one layout (`DocsLayout.razor`) with a sticky sidebar, one section per topic, copyable code blocks, and option lists; on phones the sidebar becomes an "On this page" menu.
- **Screen-by-Screen Showcase (`/showcase`)**: 13 real screenshots, 7 terminal screens (command menu, `/load` wizard, non-interactive mode, `/mcp`, `/presets`, `/settings`, `--help`) and 6 desktop screens (engine selection, model and profile, preset load, chat test, settings, MCP registration), each with its explanation and command.
- **Interactive Command Builder (`CommandBuilder.razor`)**: Visual configurator for engine, model, context size (with `8k`, `16k`, `32k`, `64k` shorthand), load profile, and execution flags with instant syntax preview and one-click clipboard copying.
- **CLI and Desktop Documentation (`/commands`)**: A section for every slash command (`/load`, `/chat`, `/mcp`, `/presets`, `/serve`, `/settings`, `/help`, `/exit`), automation examples, an options reference with every flag grouped by topic, and the desktop app screen by screen.
- **Load Profiles Breakdown**: Side-by-side comparison matrix for `speed`, `balanced` (default), and `quality` tuning profiles, detailing KV cache quantization targets (`f16`, `q8_0`, `q4_0`) and selection trade-offs.
- **Architecture & System Persistence**: Clear breakdowns explaining GGUF tensor inspection, multi-GPU allocation by PCIe bandwidth, RAM offloading for MoE models, and standard OS preset persistence directories (`~/.config/llm-tuner/presets/` on Linux/macOS, `%APPDATA%\llm-tuner\presets\` on Windows).
- **Brand Design System**: Dedicated dark mode interface built with custom CSS design tokens reflecting the official brand palette: electric cyan (`#00e5ff`), purple (`#9a00ff`), neon lime (`#65ff00`), and electric red (`#ff1a00`), alongside Ubuntu and Ubuntu Mono typography and Icons8 Line Awesome iconography.
- **Bilingual Documentation (EN / ES)**: Real-time bilingual toggle without page reload powered by `DocumentationDataService`.

---

## Tech Stack

| Layer | Technology |
|---|---|
| **Framework** | [Blazor WebAssembly](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) (.NET 10.0) |
| **Packages** | `Microsoft.AspNetCore.Components.WebAssembly` (10.0.12)<br>`Microsoft.AspNetCore.Components.WebAssembly.DevServer` (10.0.12) |
| **Hosting** | [Firebase Hosting](https://firebase.google.com/products/hosting) (`llm-tuner-b9184`) |
| **Styling** | Native CSS custom properties with dedicated dark mode & brand palette tokens |
| **Typography** | Ubuntu & Ubuntu Mono (via Google Fonts) |
| **Iconography** | Icons8 Line Awesome |

---

## Project Structure

```
llm-tuner-web-site/
├── Pages/
│   ├── Home.razor           # Hero section, quick install tabs, and performance telemetry
│   ├── WhatIsIt.razor       # Problem statement, philosophy, and comparison vs guessing
│   ├── Showcase.razor       # Screen-by-screen CLI and desktop screenshots
│   ├── Commands.razor       # CLI and desktop documentation plus the command builder
│   ├── Profiles.razor       # Detailed breakdown of speed, balanced, and quality profiles
│   ├── Architecture.razor   # Memory modeling, multi-GPU split, MoE offloading, and preset paths
│   └── NotFound.razor       # 404 handler
├── Components/
│   ├── CommandBuilder.razor # Live interactive CLI command generator with shorthand support
│   ├── DocsLayout.razor     # Shared docs layout: sidebar, sections, code blocks, options
│   ├── ProfileComparator.razor# Matrix comparing profile constraints and KV cache types
│   ├── CopyButton.razor     # One-click clipboard copy button
│   ├── LanguageToggle.razor # English / Spanish switcher
│   └── ThemeToggle.razor    # Theme selector component
├── Layout/
│   └── MainLayout.razor     # Navigation bar, brand logo, version pill, and footer
├── Services/
│   ├── DocumentationDataService.cs # Bilingual data store and state event bus
│   ├── ThemeService.cs      # Theme coordinator
│   └── ClipboardService.cs  # JavaScript clipboard interop
├── Properties/
│   └── launchSettings.json  # Local development server ports (5299 / 7154)
├── wwwroot/
│   ├── css/site.css         # Global design tokens and component styling (Dark mode palette)
│   ├── js/site.js           # Clipboard and theme JS interop functions
│   ├── screenshots/         # Real CLI and desktop captures used by /showcase
│   └── index.html           # SPA host shell and font imports
├── firebase.json            # Firebase Hosting configuration and SPA rewrites
├── llm-tuner-web-site.csproj# .NET 10 Blazor project specification
└── LICENSE                  # MIT License
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (v10.0.100 or later)
- [Firebase CLI](https://firebase.google.com/docs/cli) (optional, for deployment)

### Local Development

Run the development server with hot reload:

```bash
dotnet watch
```

Open your browser and navigate to `http://localhost:5299` (or `https://localhost:7154`).

### Verification & Build

Compile check:

```bash
dotnet build
```

Publish static WebAssembly assets for production:

```bash
dotnet publish -c Release
```

The compiled static assets are generated in `bin/Release/net10.0/publish/wwwroot/`.

> [!IMPORTANT]
> `firebase.json` points directly to `bin/Release/net10.0/publish/wwwroot`. Do not override the output directory (e.g. `-o dist`), or Firebase will deploy stale or missing assets.

### Deployment to Firebase Hosting

Ensure you have authenticated with Firebase and deploy:

```bash
firebase deploy --only hosting
```

---

## Related Projects

- **[llmtuner](https://github.com/ronald32acunadev/llmtuner)** — Core npm package: CLI wizard, headless engine, hardware estimator, and Electron desktop GUI.

---

## License

[MIT](LICENSE) © 2026 Ronald Daniel Acuña Arias
