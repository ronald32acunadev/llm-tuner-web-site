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

The web app serves as the central hub for the `llm-tuner` ecosystem. It demonstrates how `llm-tuner` mathematically models GPU VRAM, benchmarks candidate KV cache quantizations, streams real-time hardware telemetry, and configures local LLM runtimes (LM Studio and Ollama) with zero guesswork.

The portal provides interactive simulations of the CLI and Desktop interfaces, an interactive command builder with context shorthand support, an exhaustive parameter reference, profile comparison matrices, and system architecture breakdowns.

---

## Key Features

- **Independent Routed Views**: Dedicated pages for each core area (`/`, `/what`, `/showcase`, `/commands`, `/profiles`, `/architecture`).
- **Interactive Visual Showcase**:
  - **Terminal CLI Mockup (`TerminalMockup.razor`)**: Step-by-step interactive tabs for the guided wizard (`llm-tuner`), headless one-liner execution, saved presets inspection (`--presets`), interactive settings drawer (`/settings`), and help manual (`--help`).
  - **Desktop App Mockup (`DesktopAppMockup.razor`)**: Full Electron GUI showcase featuring continuous hardware telemetry (Dual RTX 5070), model selection, real-time benchmark progress, live token generation tester, and automated update notifications.
- **Interactive Command Builder (`CommandBuilder.razor`)**: Visual configurator for engine, model, context size (with `8k`, `16k`, `32k`, `64k` shorthand), load profile, and execution flags with instant syntax preview and one-click clipboard copying.
- **Complete CLI Reference**: Structured documentation covering all supported arguments (`--engine`, `--model`, `--ctx`, `--profile`, `--force`, `--candidates`, `--yes`, `--dry-run`, `--presets`, `--json`, `--lang`, `--theme`, `/settings`, `--help`, `--version / -v`).
- **Load Profiles Breakdown**: Side-by-side comparison matrix for `speed`, `balanced`, and `quality` tuning profiles, detailing KV cache quantization targets (`f16`, `q8_0`, `q4_0`) and selection criteria.
- **Real-Time Telemetry & Progress**: Live telemetry sampler (RAM, GPU VRAM peak, CPU) with animated progress bars during model offloading and benchmarks in both CLI and Desktop.
- **Automated Update Checking**: Zero-overhead background check with 24h caching via npm registry, notifying users when a newer `llm-tuner` release is published.
- **Architecture & System Persistence**: Clear diagrams explaining GGUF tensor inspection, multi-GPU allocation by PCIe bandwidth, RAM offloading for MoE models, and standard OS preset persistence directories.
- **Brand Design System**: Tailored theme engine (Dark, Light, System) styled with electric cyan (`#00e5ff`), purple (`#9a00ff`), neon lime (`#65ff00`), and electric red (`#ff1a00`), alongside Ubuntu and Ubuntu Mono typography and Icons8 Line Awesome icons.
- **Bilingual Documentation (EN / ES)**: Real-time bilingual toggle without page reload via `DocumentationDataService`.

---

## Tech Stack

| Layer | Technology |
|---|---|
| **Framework** | [Blazor WebAssembly](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) (.NET 10.0) |
| **Packages** | `Microsoft.AspNetCore.Components.WebAssembly` (10.0.12) |
| **Hosting** | [Firebase Hosting](https://firebase.google.com/products/hosting) (`llm-tuner-b9184`) |
| **Styling** | Custom CSS design tokens with Dark/Light theme switching & brand palette |
| **Typography** | Ubuntu & Ubuntu Mono |
| **Iconography** | Icons8 Line Awesome |

---

## Project Structure

```
llm-tuner-web-site/
├── Pages/
│   ├── Home.razor           # Hero section, quick install tabs, and performance telemetry
│   ├── WhatIsIt.razor       # Problem statement, philosophy, and comparison vs guessing
│   ├── Showcase.razor       # Interactive Terminal and Desktop GUI mockup switcher
│   ├── Commands.razor       # Interactive command builder and CLI argument reference
│   ├── Profiles.razor       # Detailed breakdown of speed, balanced, and quality profiles
│   ├── Architecture.razor   # Memory modeling, multi-GPU split, MoE offloading, and preset paths
│   └── NotFound.razor       # 404 handler
├── Components/
│   ├── CommandBuilder.razor # Live interactive CLI command generator with shorthand support
│   ├── TerminalMockup.razor # Tabbed terminal showcase with high-res captures
│   ├── DesktopAppMockup.razor# Tabbed Electron GUI showcase with high-res captures
│   ├── ProfileComparator.razor# Matrix comparing profile constraints and KV cache types
│   ├── CopyButton.razor     # One-click clipboard copy button
│   ├── LanguageToggle.razor # English / Spanish switcher
│   └── ThemeToggle.razor    # Dark / Light / System theme selector
├── Layout/
│   └── MainLayout.razor     # Navigation bar, mobile layout, and footer
├── Services/
│   ├── DocumentationDataService.cs # Bilingual data store and state event bus
│   ├── ThemeService.cs      # LocalStorage-backed theme coordinator
│   └── ClipboardService.cs  # JavaScript clipboard interop
├── wwwroot/
│   ├── css/site.css         # Global design tokens and component styling
│   ├── screenshots/         # High-resolution captures for terminal and desktop mockups
│   └── index.html           # SPA host shell and font imports
├── firebase.json            # Firebase Hosting configuration and SPA rewrites
└── llm-tuner-web-site.csproj# .NET 10 Blazor project specification
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

Open your browser and navigate to `http://localhost:5000` (or the port specified in terminal output).

### Building for Production

Compile and publish the static WebAssembly bundle:

```bash
dotnet publish -c Release
```

The compiled static assets are generated in `bin/Release/net10.0/publish/wwwroot/`.

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

[MIT](LICENSE) © Ronald Daniel Acuña Arias
