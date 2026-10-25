# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Agent rules

@AGENTS.md

`AGENTS.md` is authoritative for language (English everywhere except translations) and for code authorship: source code is synthesized through the local-model MCP servers (`pumbastudio`, then `pumbalama`) and the host model writes code only when both are unavailable. `.agents/rules/AGENTS.md` is a copy of the same rules for agents that read that folder; keep the two in sync.

## What this is

Marketing and documentation site for the [`llm-tuner`](https://github.com/ronald32acunadev/llmtuner) npm package. It is a standalone Blazor WebAssembly app (.NET 10, `Microsoft.NET.Sdk.BlazorWebAssembly`) with no backend, no API calls, and no test project. Everything ships as static assets to Firebase Hosting.

The site describes the CLI and desktop app of a different repository. Flags, defaults, profiles, and the version pill in `Layout/MainLayout.razor` are hand-copied from `llmtuner`; nothing here is generated from it, so check that repository when content accuracy matters.

## Commands

```bash
dotnet watch                  # dev server with hot reload (http://localhost:5299, https://localhost:7154)
dotnet build                  # compile check; this is the only automated verification available
dotnet publish -c Release     # production build into bin/Release/net10.0/publish/wwwroot
firebase deploy --only hosting  # deploys the publish output (project llm-tuner-b9184)
```

`firebase.json` points `hosting.public` at `bin/Release/net10.0/publish/wwwroot`, so publish with the default output path. Passing `-o dist` produces a build that Firebase will not pick up. All routes are rewritten to `/index.html` for client-side routing.

There are no tests and no linter. Verify changes with `dotnet build` and by loading the affected route in the browser.

## Architecture

### Content and language

`Services/DocumentationDataService.cs` is the single source of structured content (CLI commands, load profiles, execution modes, typed by `Models/SiteModels.cs`) and also owns the current language (`"en"` or `"es"`). It is registered as scoped, which in WebAssembly means one instance for the lifetime of the tab. The language is held in memory only and resets to English on reload.

There are no resource files or i18n catalogs. Localization is done inline in two ways:

- Data getters branch on `isEs` and return already-localized objects, so components must call them again after a language change rather than cache the result in a field.
- Markup uses `@(DocData.CurrentLanguage == "es" ? "..." : "...")` at each string.

Every page and any component that renders localized text follows the same lifecycle: inject `DocumentationDataService`, subscribe to `OnLanguageChanged` in `OnInitialized`, call `InvokeAsync(StateHasChanged)` in the handler, and unsubscribe in `Dispose` (`@implements IDisposable`). A new page that skips this will not update when the language toggle is used. Technical values such as flags, examples, and identifiers stay in English in both languages.

### Routing and layout

Each section is its own routed page under `Pages/` (`/`, `/what`, `/showcase`, `/commands`, `/profiles`, `/architecture`). `Home.razor` answers both `/` and `/overview`. `App.razor` sets `MainLayout` as the default layout and `Pages/NotFound.razor` as the not-found page. Adding a page also means adding its `NavLink` (with both labels) in `Layout/MainLayout.razor`.

`Components/` holds the interactive pieces embedded in pages: the terminal and desktop app mockups, the command builder, and the profile comparator. They are self-contained Razor components with local state; there is no shared state container beyond the two services.

### JS interop

`wwwroot/js/site.js` exposes a single `window.llmTuner` object. `ClipboardService` and `ThemeService` are thin wrappers over it and swallow interop failures. New browser APIs should be added to that object and wrapped in a service rather than called through `IJSRuntime` from components.

### Styling

`wwwroot/css/site.css` is the design system: raw palette tokens in `:root` (`--bg`, `--surface`, `--accent`, ...) plus semantic aliases (`--color-background`, `--color-text-weak`, `--color-border`, ...) and shared classes such as `panel`, `pill`, `cta-button`, and `navbar`. `wwwroot/css/app.css` only contains Blazor template leftovers (validation, error UI, loading spinner).

Components and pages are styled mostly with inline `style` attributes that reference the semantic `--color-*` and `--font-*` variables. Follow that convention and use the variables instead of hard-coded colors. There is no scoped `.razor.css` in the project.

Icons are Line Awesome classes (`las la-*`, `lab la-*`) and fonts are Ubuntu / Ubuntu Mono, both loaded from CDNs in `wwwroot/index.html`.

### Theme switching is not wired

The stylesheet is dark only. `ThemeService`, `Components/ThemeToggle.razor`, and the `setTheme` interop set a `data-theme` attribute, but no CSS selector reads it and `ThemeToggle` is not rendered anywhere. Light or system themes need a token override block in `site.css` and the toggle placed in the layout before they do anything.

## Known inconsistencies

- `README.md` and `README.es.md` (its translation; keep both in sync) describe a working Dark/Light/System theme engine with LocalStorage persistence, which does not exist (see above). `README.es.md` also still says port 5000.
- The "Project Context" section of `AGENTS.md` describes a generic setup template, not this site; ignore it in favor of this file.

## Syncing with llm-tuner

When `llm-tuner` changes (flags, profiles, version, screenshots), use the project skill `.claude/skills/sync-llmtuner/` instead of editing by hand. The version also appears in the README badges, not only in `Layout/MainLayout.razor`.
