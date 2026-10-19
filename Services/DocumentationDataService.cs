using llm_tuner_web_site.Models;

namespace llm_tuner_web_site.Services;

public class DocumentationDataService
{
    private string _currentLanguage = "en";

    public string CurrentLanguage => _currentLanguage;
    public event Action? OnLanguageChanged;

    public void SetLanguage(string lang)
    {
        if (_currentLanguage != lang && (lang == "en" || lang == "es"))
        {
            _currentLanguage = lang;
            OnLanguageChanged?.Invoke();
        }
    }

    public List<CliCommand> GetCommands()
    {
        bool isEs = _currentLanguage == "es";
        return new List<CliCommand>
        {
            new()
            {
                Flag = "/load",
                Category = "Commands",
                DefaultValue = "Interactive menu",
                Description = isEs ? "Sintoniza y carga un modelo con la configuración óptima para tu hardware. Es el comando por defecto cuando pasas banderas de sintonización." : "Tunes and loads a model with optimal hardware settings. It is the default command when tuning flags are passed.",
                Example = "llm-tuner /load"
            },
            new()
            {
                Flag = "/mcp",
                Category = "Commands",
                DefaultValue = "Interactive",
                Description = isEs ? "Conecta tu motor local (LM Studio u Ollama) como servidor MCP en Claude Desktop, Claude Code, OpenCode, Antigravity, VS Code y ChatGPT / Codex. Acepta --engine y --name para saltar pasos." : "Connects your local engine (LM Studio or Ollama) as an MCP server to Claude Desktop, Claude Code, OpenCode, Antigravity, VS Code, and ChatGPT / Codex. Accepts --engine and --name to skip steps.",
                Example = "llm-tuner /mcp --engine lmstudio --name pumbastudio"
            },
            new()
            {
                Flag = "/presets",
                Category = "Commands",
                DefaultValue = "Interactive",
                Description = isEs ? "Lista los presets guardados (equivalente a --presets)." : "Lists saved presets (same as --presets).",
                Example = "llm-tuner /presets"
            },
            new()
            {
                Flag = "/help · /exit",
                Category = "Commands",
                DefaultValue = "Interactive",
                Description = isEs ? "/help muestra la ayuda y /exit cierra el menú interactivo. Sin argumentos, llm-tuner abre un menú persistente con todos los comandos y Backspace vuelve al paso anterior." : "/help shows usage and /exit closes the interactive menu. Without arguments, llm-tuner opens a persistent menu with every command, and Backspace goes back one step.",
                Example = "llm-tuner /help"
            },
            new()
            {
                Flag = "--engine <lmstudio|ollama>",
                Category = "Engine",
                DefaultValue = "Interactive prompt",
                Description = isEs ? "Selecciona el motor de ejecución local (LM Studio u Ollama). Si no está instalado, ofrece instalación automática." : "Selects the local execution engine (LM Studio or Ollama). Offers automated installation if not found.",
                Example = "llm-tuner --engine lmstudio"
            },
            new()
            {
                Flag = "--model <key>",
                Category = "Model",
                DefaultValue = "Interactive prompt",
                Description = isEs ? "Clave o identificador del modelo descargado (ej: qwen/qwen2.5-coder-32b o qwen2.5-coder:32b)." : "Downloaded model key or tag (e.g., qwen/qwen2.5-coder-32b or qwen2.5-coder:32b).",
                Example = "llm-tuner --model qwen/qwen2.5-coder-32b"
            },
            new()
            {
                Flag = "--ctx <tokens|shorthand>",
                Category = "Configuration",
                DefaultValue = "16k / 16384",
                Description = isEs ? "Tamaño de la ventana de contexto. Admite números enteros (>= 512) o sufijos abreviados como 4k, 8k, 16k, 32k o 1m." : "Context window length. Accepts token integers (>= 512) or shorthand suffixes such as 4k, 8k, 16k, 32k, or 1m.",
                Example = "llm-tuner --ctx 16k"
            },
            new()
            {
                Flag = "--profile <speed|balanced|quality>",
                Category = "Optimization",
                DefaultValue = "balanced",
                Description = isEs ? "Perfil de carga objetivo: 'speed' (máximo t/s), 'balanced' (equilibrio tps/precisión KV), 'quality' (máxima fidelidad f16/q8 sin descarte de GPU)." : "Load profile objective: 'speed' (max TPS), 'balanced' (speed weighted by KV quality), 'quality' (least precision loss fully on GPU).",
                Example = "llm-tuner --profile quality"
            },
            new()
            {
                Flag = "--force",
                Category = "Execution",
                DefaultValue = "false",
                Description = isEs ? "Fuerza la medición y benchmark en vivo incluso si ya existe un preset guardado para ese hardware/modelo/contexto/perfil." : "Forces live measurement and benchmarking even if a matching preset already exists.",
                Example = "llm-tuner --force"
            },
            new()
            {
                Flag = "--candidates <n>",
                Category = "Execution",
                DefaultValue = "3",
                Description = isEs ? "Cantidad de configuraciones candidatas prometedoras a medir en vivo durante el tuning (por defecto 3)." : "Number of candidate configurations to benchmark during tuning (default 3).",
                Example = "llm-tuner --candidates 4"
            },
            new()
            {
                Flag = "--yes",
                Category = "Automation",
                DefaultValue = "false",
                Description = isEs ? "Modo no interactivo: aplica automáticamente la configuración ganadora sin solicitar confirmación manual." : "Non-interactive flag: applies winning configuration immediately without asking for user confirmation.",
                Example = "llm-tuner --yes"
            },
            new()
            {
                Flag = "--dry-run",
                Category = "Automation",
                DefaultValue = "false",
                Description = isEs ? "Ejecuta los cálculos y mediciones, muestra qué archivos y variables cambiarían, pero no modifica nada ni guarda preset." : "Computes and measures, shows proposed file and environment changes without applying or saving preset.",
                Example = "llm-tuner --dry-run"
            },
            new()
            {
                Flag = "--presets",
                Category = "Presets",
                DefaultValue = "false",
                Description = isEs ? "Lista todos los presets guardados localmente, filtrables por motor o modelo." : "Lists all cached presets saved on this machine, filterable by engine or model.",
                Example = "llm-tuner --presets"
            },
            new()
            {
                Flag = "--json",
                Category = "Output",
                DefaultValue = "false",
                Description = isEs ? "Emite la salida final y el plan de carga en formato JSON estructurado para scripts o integraciones CI." : "Emits final benchmark results and load plan in structured JSON format for scripting and CI.",
                Example = "llm-tuner --json"
            },
            new()
            {
                Flag = "--lang <en|es>",
                Category = "Settings",
                DefaultValue = "en",
                Description = isEs ? "Configura el idioma de la interfaz (English o Español) y lo guarda en settings.json." : "Sets interface language (English or Spanish) and persists it to settings.json.",
                Example = "llm-tuner --lang es"
            },
            new()
            {
                Flag = "--theme <system|light|dark>",
                Category = "Settings",
                DefaultValue = "system",
                Description = isEs ? "Configura el tema visual de la interfaz de terminal y lo guarda en settings.json." : "Sets terminal interface visual theme and persists it to settings.json.",
                Example = "llm-tuner --theme dark"
            },
            new()
            {
                Flag = "/settings",
                Category = "Commands",
                DefaultValue = "Interactive",
                Description = isEs ? "Abre el menú interactivo de configuración de idioma y tema en la terminal." : "Launches the interactive terminal settings menu for language and theme preferences.",
                Example = "llm-tuner /settings"
            },
            new()
            {
                Flag = "--help / -h",
                Category = "General",
                DefaultValue = "false",
                Description = isEs ? "Muestra la ayuda completa con todas las opciones disponibles y la ruta de presets." : "Displays complete usage syntax, flags, and presets storage directory.",
                Example = "llm-tuner --help"
            },
            new()
            {
                Flag = "--version / -v",
                Category = "General",
                DefaultValue = "false",
                Description = isEs ? "Imprime la versión instalada de llm-tuner y sale (ej: 0.4.2)." : "Prints the installed version of llm-tuner and exits (e.g., 0.4.2).",
                Example = "llm-tuner --version"
            }
        };
    }

    public List<LoadProfileInfo> GetProfiles()
    {
        bool isEs = _currentLanguage == "es";
        return new List<LoadProfileInfo>
        {
            new()
            {
                Id = "speed",
                Name = isEs ? "Velocidad (Speed)" : "Speed",
                Tagline = isEs ? "Máxima tasa de generación de tokens por segundo" : "Maximum generation tokens per second",
                Description = isEs
                    ? "Prioriza el throughput por encima de todo. Permite la cuantización de pesos más ligera disponible y cualquier tipo de caché KV (f16, q8_0, q4_0)."
                    : "Prioritizes throughput above all. Uses the lightest downloaded model variant and any KV cache type (f16, q8_0, q4_0).",
                KvTypes = "f16, q8_0, q4_0",
                TargetVariant = isEs ? "Variante más ligera descargada" : "Lightest downloaded variant",
                TradeOff = isEs ? "Sacrifica fidelidad/precisión por velocidad pura" : "Trades answer precision for raw speed",
                RecommendedCondition = isEs ? "Modelos pesados donde cada t/s cuenta o consultas masivas" : "Heavy models where every t/s matters or bulk processing",
                IsDefault = false
            },
            new()
            {
                Id = "balanced",
                Name = isEs ? "Equilibrado (Balanced)" : "Balanced",
                Tagline = isEs ? "Velocidad ponderada por calidad del KV cache (Por defecto)" : "Speed weighted by KV cache quality (Default)",
                Description = isEs
                    ? "El punto óptimo para la mayoría de usuarios. Respeta la variante seleccionada y busca la configuración más rápida con contexto lleno, ponderando el tipo de caché KV."
                    : "The sweet spot for most developers. Uses the selected model variant and finds the fastest full-context configuration, weighted by KV quality.",
                KvTypes = "f16, q8_0, q4_0",
                TargetVariant = isEs ? "Variante seleccionada por el usuario" : "User-selected variant",
                TradeOff = isEs ? "Punto medio: ni el más agresivo ni el más pesado" : "Middle ground: optimal balance between speed and precision",
                RecommendedCondition = isEs ? "Perfil recomendado por defecto en el asistente y ejecuciones" : "Default recommended profile for everyday coding and reasoning",
                IsDefault = true
            },
            new()
            {
                Id = "quality",
                Name = isEs ? "Calidad (Quality)" : "Quality",
                Tagline = isEs ? "Mínima pérdida que aún corre 100% en GPU" : "Least precision loss that still fits 100% in GPU",
                Description = isEs
                    ? "Prioriza respuestas exactas y precisas. Selecciona la variante descargada más pesada que quepa en VRAM y fuerza caché KV f16 o q8_0 (excluye q4_0)."
                    : "Prioritizes answer precision and coherence. Selects the heaviest variant that fits into VRAM and enforces precise KV cache (f16 or q8_0, omits q4_0).",
                KvTypes = "f16, q8_0",
                TargetVariant = isEs ? "Variante más pesada que quepa en GPU" : "Heaviest variant fitting 100% in GPU",
                TradeOff = isEs ? "Menor velocidad de generación si la VRAM es justa" : "Lower tokens/sec generation speed in exchange for fidelity",
                RecommendedCondition = isEs ? "Hardware con VRAM sobrante (ej: 2x GPUs o tarjetas de 16GB+)" : "Hardware with generous VRAM headroom (e.g. multi-GPU or 16GB+ VRAM)",
                IsDefault = false
            }
        };
    }

    public List<ExecutionModeInfo> GetExecutionModes()
    {
        bool isEs = _currentLanguage == "es";
        return new List<ExecutionModeInfo>
        {
            new()
            {
                Id = "wizard",
                Name = isEs ? "Menú Interactivo y Slash Commands" : "Interactive Menu & Slash Commands",
                Description = isEs
                    ? "Sin argumentos abre un menú persistente con /load, /mcp, /presets, /settings, /help y /exit. Backspace vuelve al paso anterior y /load detecta motores, lista modelos por peso, calcula capas GPU y muestra la carga en vivo como en la app de escritorio."
                    : "Without arguments it opens a persistent menu with /load, /mcp, /presets, /settings, /help, and /exit. Backspace steps back, and /load detects engines, sorts models by size, calculates GPU layers, and shows a desktop-like live loading animation.",
                Command = "llm-tuner",
                BinTarget = "src/cli/index.js",
                Badge = "CLI Wizard"
            },
            new()
            {
                Id = "desktop",
                Name = isEs ? "Aplicación de Escritorio (Desktop)" : "Desktop Electron App",
                Description = isEs
                    ? "Aplicación nativa en Electron con telemetría continua de hardware (VRAM, CPU, PCIe), barras de progreso animadas, aviso de versiones nuevas, tabla de benchmark en vivo, probador de chat integrado y botón \"Agregar como MCP\" por motor."
                    : "Native Electron GUI with continuous hardware telemetry (VRAM, CPU, PCIe), animated progress bars, update notifications, live benchmark progress table, built-in chat tester, and an \"Add as MCP\" button per engine.",
                Command = "llm-tuner-desktop",
                BinTarget = "electron/launch.js",
                Badge = "GUI App"
            },
            new()
            {
                Id = "direct",
                Name = isEs ? "CLI Directo No Interactivo" : "Non-Interactive Headless CLI",
                Description = isEs
                    ? "Automatización pura para scripts y pipelines: pasa motor, modelo, contexto (con sufijos k/m como 16k o 32k) y perfil con la bandera --yes para sintonizar y cargar sin interrupciones."
                    : "One-line automated execution for scripts and CI/CD pipelines: pass engine, model, context shorthand (e.g. 16k, 32k), and profile with --yes for seamless tuning.",
                Command = "llm-tuner --engine lmstudio --model qwen/qwen2.5-coder-32b --ctx 16k --profile quality --yes",
                BinTarget = "src/cli/index.js",
                Badge = "Automation"
            },
            new()
            {
                Id = "mcp",
                Name = isEs ? "Integración MCP con Agentes de IA" : "AI Agent MCP Integration",
                Description = isEs
                    ? "Registra LM Studio u Ollama como servidor MCP en Claude Desktop, Claude Code, OpenCode, Antigravity, VS Code y ChatGPT / Codex. Detecta qué agentes están instalados, permite nombrar el servidor y edita sus configuraciones JSON o TOML de forma segura."
                    : "Registers LM Studio or Ollama as an MCP server in Claude Desktop, Claude Code, OpenCode, Antigravity, VS Code, and ChatGPT / Codex. Detects installed agents, lets you name the server, and safely edits their JSON or TOML configs.",
                Command = "llm-tuner /mcp",
                BinTarget = "src/core/mcp/index.js",
                Badge = "MCP"
            },
            new()
            {
                Id = "settings",
                Name = isEs ? "Menú de Configuración" : "Interactive Settings Menu",
                Description = isEs
                    ? "Configuración rápida de preferencias globales del usuario: cambia el idioma de la aplicación (EN / ES) o el tema visual (System, Light, Dark) persistido en disco."
                    : "Terminal preference manager: switch application language (English / Español) and visual theme (System, Light, Dark) persisted in settings.json.",
                Command = "llm-tuner /settings",
                BinTarget = "src/cli/index.js",
                Badge = "Config"
            },
            new()
            {
                Id = "presets",
                Name = isEs ? "Gestor de Presets Guardados" : "Presets Cache Inspection",
                Description = isEs
                    ? "Inspecciona los presets almacenados en tu sistema por huella digital de hardware, motor, modelo, contexto y perfil. Permite cargas instantáneas en ~20s sin volver a medir."
                    : "Inspect cached hardware presets stored on your machine. Enables instant ~20s model loads with zero redundant benchmarking.",
                Command = "llm-tuner /presets",
                BinTarget = "src/core/presets.js",
                Badge = "Cache"
            }
        };
    }
}
