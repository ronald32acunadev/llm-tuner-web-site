# LLM Tuner — Sitio Web de Documentación y Muestrario Visual

[![Live Web](https://img.shields.io/badge/Firebase-Hosting-FFCA28?logo=firebase&logoColor=black)](https://llm-tuner-b9184.web.app)
[![Version](https://img.shields.io/badge/versión-v0.4.2-00e5ff)](https://github.com/ronald32acunadev/llmtuner/releases/tag/v0.4.2)
[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Blazor WebAssembly](https://img.shields.io/badge/Blazor-WebAssembly-512BD4?logo=blazor&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

[English](README.md) · [Español](README.es.md)

Sitio web oficial de documentación y portal interactivo para el paquete npm [`llm-tuner`](https://github.com/ronald32acunadev/llmtuner) (v0.4.2), desarrollado con **Blazor WebAssembly** en **.NET 10** y desplegado en **Firebase Hosting**.

---

## Descripción General

Esta aplicación web actúa como el centro de documentación y demostración del ecosistema `llm-tuner`. Explica cómo `llm-tuner` modela matemáticamente la memoria VRAM de las GPUs, evalúa cuantizaciones candidatas del KV cache, transmite telemetría en tiempo real y aplica configuraciones óptimas en motores locales (LM Studio y Ollama) de forma determinista y sin conjeturas.

El portal ofrece simulaciones interactivas de las interfaces de consola (CLI) y escritorio (Desktop), un generador de comandos con soporte para sufijos abreviados de contexto, referencia detallada de parámetros, comparativa de perfiles y la arquitectura técnica del sistema.

---

## Características Principales

- **Vistas Enrutadas Independientes**: Secciones modulares con rutas dedicadas (`/`, `/what`, `/showcase`, `/commands`, `/profiles`, `/architecture`).
- **Muestrario Visual Interactivo**:
  - **Simulador de Terminal CLI (`TerminalMockup.razor`)**: Pestañas interactivas paso a paso para el asistente guiado (`llm-tuner`), ejecución headless en una sola línea, inspección de presets (`--presets`), menú de configuración interactivo (`/settings`) y manual de ayuda (`--help`).
  - **Simulador de App de Escritorio (`DesktopAppMockup.razor`)**: Muestrario completo de la interfaz Electron con telemetría de hardware (Dual RTX 5070), selección de modelo, progreso del benchmark en tiempo real, probador de generación de tokens en vivo y aviso automático de actualizaciones.
- **Generador Interactivo de Comandos (`CommandBuilder.razor`)**: Configurador visual de motor, modelo, contexto (con sufijos `8k`, `16k`, `32k`, `64k`), perfil y modificadores, con vista previa de sintaxis inmediata y copia al portapapeles.
- **Referencia Completa del CLI**: Documentación exhaustiva de flags (`--engine`, `--model`, `--ctx`, `--profile`, `--force`, `--candidates`, `--yes`, `--dry-run`, `--presets`, `--json`, `--lang`, `--theme`, `/settings`, `--help`, `--version / -v`).
- **Análisis de Perfiles de Carga**: Matriz interactiva que compara los objetivos de `speed`, `balanced` y `quality`, detallando cuantizaciones de KV cache (`f16`, `q8_0`, `q4_0`) y reglas de selección.
- **Telemetría y Barras de Progreso en Vivo**: Muestreo continuo de memoria RAM, picos de VRAM y CPU con barras de progreso animadas durante el benchmarking y la carga del modelo.
- **Aviso Automático de Nuevas Versiones**: Verificación en segundo plano sin latencia que consulta el registro de npm (con caché de 24h) y notifica al usuario cuando hay una actualización disponible.
- **Arquitectura y Persistencia**: Diagramas técnicos que detallan la inspección de tensores GGUF, balanceo multi-GPU por ancho de banda PCIe, offload a RAM para modelos MoE y rutas estándar de almacenamiento de presets por sistema operativo.
- **Sistema de Diseño y Marca**: Motor de temas (Oscuro, Claro, Sistema) diseñado con la paleta oficial (cian eléctrico `#00e5ff`, púrpura `#9a00ff`, lima neón `#65ff00`, rojo eléctrico `#ff1a00`), tipografía Ubuntu y Ubuntu Mono e iconografía Icons8 Line Awesome.
- **Documentación Bilingüe (EN / ES)**: Cambio de idioma en tiempo real sin recargar la página mediante `DocumentationDataService`.

---

## Pila Tecnológica

| Capa | Tecnología |
|---|---|
| **Framework** | [Blazor WebAssembly](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) (.NET 10.0) |
| **Paquetes** | `Microsoft.AspNetCore.Components.WebAssembly` (10.0.12) |
| **Hospedaje** | [Firebase Hosting](https://firebase.google.com/products/hosting) (`llm-tuner-b9184`) |
| **Estilos** | Variables CSS nativas con paleta de marca y soporte para temas Claro / Oscuro |
| **Tipografía** | Ubuntu y Ubuntu Mono |
| **Iconos** | Icons8 Line Awesome |

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
│   ├── TerminalMockup.razor # Simulador de terminal con capturas en alta resolución
│   ├── DesktopAppMockup.razor# Simulador de Electron con capturas en alta resolución
│   ├── ProfileComparator.razor# Matriz comparativa de restricciones y tipos de KV cache
│   ├── CopyButton.razor     # Botón reutilizable de copiado al portapapeles
│   ├── LanguageToggle.razor # Conmutador bilingüe Inglés / Español
│   └── ThemeToggle.razor    # Selector de tema Oscuro / Claro / Sistema
├── Layout/
│   └── MainLayout.razor     # Barra de navegación superior, layout responsivo y pie de página
├── Services/
│   ├── DocumentationDataService.cs # Servicio central de datos i18n y eventos de estado
│   ├── ThemeService.cs      # Gestor de tema con persistencia en LocalStorage
│   └── ClipboardService.cs  # Interoperabilidad con el portapapeles del navegador
├── wwwroot/
│   ├── css/site.css         # Tokens de diseño y estilos globales
│   ├── screenshots/         # Capturas en alta resolución para los simuladores
│   └── index.html           # Plantilla host de la SPA y carga de fuentes
├── firebase.json            # Configuración de Firebase Hosting y reglas SPA
└── llm-tuner-web-site.csproj# Especificación del proyecto .NET 10 Blazor
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

Abre tu navegador en `http://localhost:5000` (o la URL indicada en la consola).

### Compilación para Producción

Compila y publica los recursos estáticos WebAssembly:

```bash
dotnet publish -c Release
```

Los artefactos listos para producción se generarán en `bin/Release/net10.0/publish/wwwroot/`.

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

[MIT](LICENSE) © Ronald Daniel Acuña Arias
