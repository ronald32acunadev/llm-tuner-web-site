---
name: sync-llmtuner
description: Sync the site with the latest changes in the llm-tuner repository (CLI and desktop app). Use when the user says llm-tuner changed, asks to update/sync/refresh the site content, the version pill, or the showcase screenshots.
---

# Sync site with llm-tuner

The site content is hand-copied from the sibling repository `../llmtuner` (the `llm-tuner` npm package). This skill brings the site up to date with it, CLI and desktop app included, and refreshes the real screenshots.

Run the whole process without asking for approval at each step. Stop only if a step fails.

## 1. Find what changed

The last synced version is the version pill in `Layout/MainLayout.razor` (`vX.Y.Z`).

```bash
grep -o 'v[0-9]*\.[0-9]*\.[0-9]*' Layout/MainLayout.razor
node -p "require('../llmtuner/package.json').version"
git -C ../llmtuner log --oneline --format='%h %s' | grep -n 'bump version'
```

Find the commit that bumped the llm-tuner version to the site's version, then list everything after it:

```bash
git -C ../llmtuner log --oneline <bump-commit>..HEAD
git -C ../llmtuner diff --stat <bump-commit>..HEAD -- src electron
git -C ../llmtuner diff <bump-commit>..HEAD -- README.md
```

If the versions match and nothing user-facing changed, report that the site is current and stop.

Primary sources for user-facing facts. Read these, not the whole diff:

| What | Where in `../llmtuner` |
|------|------------------------|
| Commands, flags, help text (EN/ES) | `src/i18n/en.js`, `src/i18n/es.js` (`cli.help`, `cli.*`, `web.*`) |
| Command routing and accepted args | `src/cli/router.js`, `src/cli/commands/*.js` |
| Desktop UI | `src/web/public/index.html`, `src/web/public/app.js` |
| Feature summary | `README.md`, `README.es.md` |

Skip internal-only changes (refactors, engine fixes with no visible effect, tests).

## 2. Update the site content

Every user-facing string exists in English and Spanish, and both must be updated. Flags, commands, and identifiers stay in English in both languages.

| Change in llm-tuner | Site file |
|---------------------|-----------|
| Version | `Layout/MainLayout.razor` pill, `--version` example in `Services/DocumentationDataService.cs` |
| Commands and flags | `GetCommands()` in `Services/DocumentationDataService.cs`; a new `Category` also goes into `commandCategories` in `Pages/Commands.razor` |
| Execution modes | `GetExecutionModes()` in the same service |
| Load profiles | `GetProfiles()` in the same service |
| New headline feature | a pillar card in `Pages/WhatIsIt.razor` (copy an existing card) |
| Command builder options | `Components/CommandBuilder.razor` |
| Screenshot captions and tabs | `Components/TerminalMockup.razor` (`cliTabs`), `Components/DesktopAppMockup.razor` (`steps`) |

Follow the project conventions in `CLAUDE.md`: inline `DocData.CurrentLanguage == "es" ? ... : ...`, semantic `--color-*` variables, and Line Awesome icons. Source code goes through the local model first, as `AGENTS.md` requires.

## 3. Refresh screenshots

```bash
node .claude/skills/sync-llmtuner/scripts/capture.mjs --llmtuner ../llmtuner --out wwwroot/screenshots
```

The script starts the real desktop server and CLI from `../llmtuner` with an isolated `XDG_CONFIG_HOME`, so the user's settings and presets are never touched. It drives headless Chrome over CDP. Requirements: Node 22+, Chrome or Chromium (`CHROME_BIN` overrides the path), and `script` (util-linux).

It regenerates these files:

| File | Content |
|------|---------|
| `01-engine-selection.png` | Desktop engine selection with the "Add as MCP" buttons |
| `06-mcp-modal.png` | Desktop "Add as MCP Server" modal |
| `cli-01-menu.png` | CLI slash command menu (`llm-tuner`) |
| `cli-05-help.png` | `llm-tuner --help` |
| `cli-06-mcp.png` | First step of `llm-tuner /mcp` |

The remaining screenshots were taken with a real model loaded (Qwen3 Coder 30B) and are not regenerated: `02`–`05` desktop and `cli-01-wizard`, `cli-02-headless`, `cli-03-presets`, `cli-04-settings`. If llm-tuner changed those screens, tell the user they need a manual capture.

When a new screen deserves a capture, add it to `capture.mjs` (desktop steps or the `cliCaptures` list) and add the matching tab in the mockup component.

Open every regenerated image with the Read tool and check it before continuing. The image should be in dark theme, English, and show no temp paths.

## 4. Verify

```bash
dotnet build
```

Then start the `site` preview (`.claude/launch.json`, port 5299) and check `/showcase` (both tabs), `/commands`, and `/what` in English and Spanish.

## 5. Report

Summarize the llm-tuner version range synced, the site files changed, the screenshots regenerated, and anything that still needs a manual capture. Do not commit or deploy unless the user asks. Deploying means `dotnet publish -c Release` followed by `firebase deploy --only hosting`.
