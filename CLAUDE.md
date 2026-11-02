# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Agent rules

@AGENTS.md

`AGENTS.md` is authoritative for language (English everywhere except translations), for the backlog workflow (stories saved to `docs/userstories/<feature>/README.md` and approved before implementation), and for code authorship: source code is synthesized through the local-model MCP servers (`pumbastudio`, then `pumbalama`), with `scripts/ask_local_llm.py` as the fallback when the MCP call times out. `.agents/rules/AGENTS.md` is a copy of the same rules for agents that read that folder; keep the two identical.

The "Project Context" section of `AGENTS.md` describes a generic setup template, not this site; this file replaces it.

## What this is

Marketing and documentation site for the [`llm-tuner`](https://github.com/ronald32acunadev/llmtuner) npm package. It is a standalone Blazor WebAssembly app (.NET 10, `Microsoft.NET.Sdk.BlazorWebAssembly`) with no backend, no API calls, and no test project. Everything ships as static assets to Firebase Hosting.

The site describes the CLI and desktop app of a different repository (checked out at `../../Desktop/llmtuner`). Flags, defaults, profiles, and the version pill in `Layout/MainLayout.razor` are hand-copied from it; nothing here is generated, so check that repository when content accuracy matters. When it changes, use the project skill `sync-llmtuner` (`.claude/skills/sync-llmtuner/`) instead of editing by hand; it also recaptures the showcase screenshots in `wwwroot/screenshots/`.

## Commands

```bash
dotnet watch                    # dev server with hot reload (http://localhost:5299, https://localhost:7154)
dotnet build                    # compile check; the only automated verification available
dotnet publish -c Release       # production build into bin/Release/net10.0/publish/wwwroot
firebase deploy --only hosting  # deploys the publish output (project llm-tuner-b9184)
```

For the in-app browser preview, `.claude/launch.json` defines the `site` configuration (`dotnet run --launch-profile http`, port 5299).

`firebase.json` points `hosting.public` at `bin/Release/net10.0/publish/wwwroot`, so publish with the default output path; `-o dist` produces a build Firebase will not pick up. All routes are rewritten to `/index.html` for client-side routing.

There are no tests and no linter. Verify changes with `dotnet build` and by loading the affected route in the browser.

## Architecture

### Content and language

`Services/DocumentationDataService.cs` is the single source of structured content (CLI commands, load profiles, execution modes, typed by `Models/SiteModels.cs`) and also owns the current language (`"en"` or `"es"`). It is registered as scoped, which in WebAssembly means one instance for the lifetime of the tab. The language is held in memory only and resets to English on reload.

There are no resource files or i18n catalogs. Localization is inline in two ways:

- Data getters branch on `isEs` and return already-localized objects, so callers must call them again after a language change rather than cache the result in a field.
- Markup uses `@(DocData.CurrentLanguage == "es" ? "..." : "...")` at each string.

Pages follow one lifecycle: inject `DocumentationDataService`, subscribe to `OnLanguageChanged` in `OnInitialized`, call `InvokeAsync(StateHasChanged)` in the handler, and unsubscribe in `Dispose` (`@implements IDisposable`). Components that render localized text follow the same lifecycle: a parent page re-rendering does not re-render a parameterless child, so a page or component that skips it keeps the old language after the toggle. Technical values such as flags, examples, and identifiers stay in English in both languages.

### Routing and layout

Each section is its own routed page under `Pages/` (`/`, `/what`, `/showcase`, `/commands`, `/profiles`, `/architecture`). `Home.razor` answers both `/` and `/overview`. `App.razor` sets `MainLayout` as the default layout and `Pages/NotFound.razor` as the not-found page. Adding a page also means adding its `NavLink` (with both labels) in `Layout/MainLayout.razor`.

`/commands`, `/showcase`, and `/profiles` are documentation pages rendered by `Components/DocsLayout.razor`: a sidebar of groups and one section per topic, from `GetDocSections()`, `GetShowcaseSections()`, and `GetProfileSections()` (`DocSection`, `DocExample`, `DocOption`, `DocGroup` in `Models/SiteModels.cs`). Change content in those methods, not in page markup. A page passes its groups, its route (used for anchors such as `commands#cli-load`), and an optional `SectionFooter` to append a component to one section (the command builder, the profile comparison table). Layout classes (`docs-*`) live in `wwwroot/css/site.css`.

`Components/` holds `DocsLayout` and the interactive pieces embedded in pages: the command builder and the profile comparator. They keep local state; there is no shared state container beyond the services.

### JS interop

`wwwroot/js/site.js` exposes a single `window.llmTuner` object. `ClipboardService` and `ThemeService` are thin wrappers over it and swallow interop failures. Add new browser APIs to that object and wrap them in a service rather than calling `IJSRuntime` from components.

### Styling

`wwwroot/css/site.css` is the design system: raw palette tokens in `:root` (`--bg`, `--surface`, `--accent`, ...) plus semantic aliases (`--color-background`, `--color-text-weak`, `--color-border`, ...) and shared classes such as `panel`, `pill`, `cta-button`, and `navbar`. `wwwroot/css/app.css` only contains Blazor template leftovers (validation, error UI, loading spinner).

Components and pages are styled mostly with inline `style` attributes that reference the semantic `--color-*` and `--font-*` variables. Follow that convention instead of hard-coding colors. There is no scoped `.razor.css` in the project.

Icons are Line Awesome classes (`las la-*`, `lab la-*`) and fonts are Ubuntu / Ubuntu Mono, both loaded from CDNs in `wwwroot/index.html`.

### Theme switching is not wired

The stylesheet is dark only. `ThemeService`, `Components/ThemeToggle.razor`, and the `setTheme` interop set a `data-theme` attribute (persisted in `localStorage` as `llmtuner-theme`), but no CSS selector reads it and `ThemeToggle` is not rendered anywhere. Light or system themes need a token override block in `site.css` and the toggle placed in the layout before they do anything.
