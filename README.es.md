# LLM Tuner — Sitio Web de Documentación y Muestrario Visual

[![Live Web](https://img.shields.io/badge/Firebase-Hosting-FFCA28?logo=firebase&logoColor=black)](https://llm-tuner-b9184.web.app)
[![Version](https://img.shields.io/badge/versión-v0.4.2-00e5ff)](https://github.com/ronald32acunadev/llmtuner/releases/tag/v0.4.2)
[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Blazor WebAssembly](https://img.shields.io/badge/Blazor-WebAssembly-512BD4?logo=blazor&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

[English](README.md) · [Español](README.es.md)

Sitio web oficial de documentación y portal visual interactivo para el paquete npm [`llm-tuner`](https://github.com/ronald32acunadev/llmtuner) (v0.4.2), desarrollado con **Blazor WebAssembly** en **.NET 10** y desplegado en **Firebase Hosting**.

---

## Descripción General

Esta aplicación web actúa como el centro de documentación y demostración visual del ecosistema `llm-tuner`. Explica cómo `llm-tuner` modela matemáticamente la memoria VRAM de las GPUs, evalúa cuantizaciones candidatas del KV cache, transmite telemetría en tiempo real, registra motores locales como servidores MCP para agentes de IA y aplica configuraciones óptimas en motores locales (LM Studio y Ollama) de forma determinista y sin conjeturas.

El portal ofrece simulaciones interactivas de alta fidelidad para las interfaces de consola (CLI) y de escritorio (Electron), un generador de comandos con soporte para sufijos abreviados de contexto (`8k`, `16k`, `32k`, `64k`), referencia exhaustiva de parámetros, matrices comparativas de perfiles y la arquitectura técnica del sistema.

---

## Características Principales

- **Vistas Enrutadas Independientes**: Secciones modulares con rutas dedicadas (`/` o `/overview`, `/what`, `/showcase`, `/commands`, `/profiles`, `/architecture`).
- **Muestrario Visual Interactivo**:
  - **Simulador de Terminal CLI (`TerminalMockup.razor`)**: 7 pestañas interactivas que cubren el menú persistente de slash commands (`llm-tuner`), asistente guiado (`/load`), ejecución headless en una sola línea, registro de servidores MCP (`/mcp`), inspección de presets (`/presets`), panel de configuración interactivo (`/settings`) y manual de ayuda (`--help`).
  - **Simulador de App de Escritorio (`DesktopAppMockup.razor`)**: Muestrario de 6 pasos de la interfaz Electron con telemetría continua de hardware (Dual RTX 5070), selección de modelo con acierto instantáneo de presets, progreso de benchmark en tiempo real, probador de inferencia en vivo, panel de preferencias y modal de configuración MCP.
- **Generador Interactivo de Comandos (`CommandBuilder.razor`)**: Configurador visual de motor, modelo, contexto (con sufijos `8k`, `16k`, `32k`, `64k`), perfil y modificadores, con vista previa de sintaxis inmediata y copiado al portapapeles en un clic.
- **Referencia Completa de CLI y Slash Commands**: Documentación estructurada de slash commands (`/load`, `/mcp`, `/presets`, `/settings`, `/help`, `/exit`) y banderas CLI (`--engine`, `--model`, `--ctx`, `--profile`, `--force`, `--candidates`, `--yes`, `--dry-run`, `--presets`, `--json`, `--lang`, `--theme`, `--help / -h`, `--version / -v`).
- **Análisis de Perfiles de Carga**: Matriz interactiva que compara los objetivos de `speed`, `balanced` (por defecto) y `quality`, detallando cuantizaciones de KV cache (`f16`, `q8_0`, `q4_0`) y compensaciones de rendimiento.
- **Arquitectura y Persistencia**: Diagramas técnicos que detallan la inspección de tensores GGUF, balanceo multi-GPU por ancho de banda PCIe, offload a RAM para modelos MoE y rutas estándar de almacenamiento de presets (`~/.config/llm-tuner/presets/` en Linux/macOS, `%APPDATA%\llm-tuner\presets\` en Windows).
- **Sistema de Diseño y Marca**: Interfaz en modo oscuro dedicada con tokens CSS que implementan la paleta oficial (cian eléctrico `#00e5ff`, púrpura `#9a00ff`, lima neón `#65ff00`, rojo eléctrico `#ff1a00`), tipografía Ubuntu y Ubuntu Mono e iconografía Icons8 Line Awesome.
- **Documentación Bilingüe (EN / ES)**: Cambio de idioma en tiempo real sin recargar la página mediante `DocumentationDataService`.

---

## Pila Tecnológica

| Capa | Tecnología |
|---|---|
| **Framework** | [Blazor WebAssembly](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) (.NET 10.0) |
| **Paquetes** | `Microsoft.AspNetCore.Components.WebAssembly` (10.0.12)<br>`Microsoft.AspNetCore.Components.WebAssembly.DevServer` (10.0.12) |
| **Hospedaje** | [Firebase Hosting](https://firebase.google.com/products/hosting) (`llm-tuner-b9184`) |
| **Estilos** | Variables CSS nativas con paleta de marca y diseño en modo oscuro dedicado |
| **Tipografía** | Ubuntu y Ubuntu Mono (vía Google Fonts) |
| **Iconografía** | Icons8 Line Awesome |

---

## Estructura del Proyecto

```
llm-tuner-web-site/
├── Pages/
│   ├── Home.razor           # Sección principal, pestañas de instalación y telemetría técnica
│   ├── WhatIsIt.razor       # Filosofía, fundamentación y comparativa frente al ensayo y error
│   ├── Showcase.razor       # Muestrario interactivo de interfaces CLI y Desktop
│   ├── Commands.razor       # Generador de comandos y tabla de referencia de parámetros
│   ├── Profiles.razor       # Análisis detallado de perfiles speed, balanced y quality
│   ├── Architecture.razor   # Modelado de memoria, partición multi-GPU, MoE y rutas de presets
│   └── NotFound.razor       # Manejador de ruta no encontrada (404)
├── Components/
│   ├── CommandBuilder.razor # Constructor interactivo de sintaxis CLI con soporte de sufijos
│   ├── TerminalMockup.razor # Simulador de terminal con capturas en alta resolución (7 pestañas)
│   ├── DesktopAppMockup.razor# Simulador de Electron con capturas en alta resolución (6 pasos)
│   ├── ProfileComparator.razor# Matriz comparativa de restricciones y tipos de KV cache
│   ├── CopyButton.razor     # Botón reutilizable de copiado al portapapeles
│   ├── LanguageToggle.razor # Conmutador bilingüe Inglés / Español
│   └── ThemeToggle.razor    # Componente selector de tema
├── Layout/
│   └── MainLayout.razor     # Barra de navegación superior, logo, versión y pie de página
├── Services/
│   ├── DocumentationDataService.cs # Servicio central de datos i18n y eventos de estado
│   ├── ThemeService.cs      # Gestor de tema
│   └── ClipboardService.cs  # Interoperabilidad con el portapapeles del navegador
├── Properties/
│   └── launchSettings.json  # Puertos del servidor de desarrollo local (5299 / 7154)
├── wwwroot/
│   ├── css/site.css         # Tokens de diseño y estilos globales (Paleta modo oscuro)
│   ├── js/site.js           # Funciones JS de interop para portapapeles y tema
│   ├── screenshots/         # Capturas en alta resolución para los simuladores
│   └── index.html           # Plantilla host de la SPA y carga de fuentes
├── firebase.json            # Configuración de Firebase Hosting y reglas SPA
├── llm-tuner-web-site.csproj# Especificación del proyecto .NET 10 Blazor
└── LICENSE                  # Licencia MIT
```

---

## Puesta en Marcha

### Requisitos Previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (v10.0.100 o superior)
- [Firebase CLI](https://firebase.google.com/docs/cli) (opcional, para despliegue)

### Desarrollo Local

Inicia el servidor de desarrollo con recarga en caliente:

```bash
dotnet watch
```

Abre tu navegador en `http://localhost:5299` (o `https://localhost:7154`).

### Verificación y Compilación

Verificación de compilación:

```bash
dotnet build
```

Compila y publica los recursos estáticos WebAssembly para producción:

```bash
dotnet publish -c Release
```

Los artefactos listos para producción se generan en `bin/Release/net10.0/publish/wwwroot/`.

> [!IMPORTANT]
> `firebase.json` apunta directamente a `bin/Release/net10.0/publish/wwwroot`. No cambies el directorio de salida (ej. `-o dist`), de lo contrario Firebase desplegará archivos desactualizados o incompletos.

### Despliegue en Firebase Hosting

Inicia sesión en Firebase y ejecuta el despliegue:

```bash
firebase deploy --only hosting
```

---

## Proyectos Relacionados

- **[llmtuner](https://github.com/ronald32acunadev/llmtuner)** — Paquete npm principal: asistente de terminal, motor de cálculo, estimador de hardware e interfaz de escritorio en Electron.

---

## Licencia

[MIT](LICENSE) © 2026 Ronald Daniel Acuña Arias
