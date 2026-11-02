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
                Description = isEs ? "En el equipo activo, elige un motor y un modelo, y los sintoniza y carga con la configuración óptima para el hardware. Es el comando por defecto cuando pasas banderas de sintonización." : "On the active computer, chooses an engine and model, then tunes and loads it with optimal hardware settings. It is the default command when tuning flags are passed.",
                Example = "llm-tuner /load"
            },
            new()
            {
                Flag = "/chat",
                Category = "Commands",
                DefaultValue = "Interactive",
                Description = isEs ? "Prueba un modelo cargado con una conversación en el equipo activo. Mantiene el historial hasta /exit; --prompt envía un mensaje y sale." : "Tests a loaded model with a conversation on the active computer. Keeps history until /exit; --prompt sends one message and exits.",
                Example = "llm-tuner /chat"
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
                Flag = "/serve",
                Category = "Commands",
                DefaultValue = "0.0.0.0:7860",
                Description = isEs ? "Comparte este equipo como servidor remoto. Toda petición requiere un token Bearer; si no pasas --token, reutiliza el guardado o genera uno y lo guarda para la próxima vez. Otros clientes CLI o de escritorio lo controlan desde la red." : "Shares this computer as a remote server. Every request requires a Bearer token; without --token it reuses the saved one or generates one and saves it for next time. Other CLI or desktop clients control it over the network.",
                Example = "llm-tuner /serve --port 7860"
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
                Description = isEs ? "Cantidad de configuraciones candidatas prometedoras a medir en vivo durante el tuning (por defecto 3). Debe ser un número entero positivo." : "Number of candidate configurations to benchmark during tuning (default 3). Must be a positive whole number.",
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
                Flag = "--prompt <text>",
                Category = "Chat",
                DefaultValue = "false",
                Description = isEs ? "Con /chat: envía un mensaje y sale. Sin una terminal también necesita --engine y --model." : "With /chat: sends one message and exits. Without a terminal it also needs --engine and --model.",
                Example = "llm-tuner /chat --engine ollama --model qwen2.5-coder:14b --prompt \"Hello\""
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
                Description = isEs ? "Abre el menú interactivo de configuración: idioma, tema, inicio automático del servidor al iniciar sesión, datos de conexión del servidor con su token y equipos guardados." : "Launches the interactive settings menu: language, theme, automatic server startup after login, server connection details with its token, and saved computers.",
                Example = "llm-tuner /settings"
            },
            new()
            {
                Flag = "--server <id|name>",
                Category = "Server",
                DefaultValue = "local",
                Description = isEs ? "Selecciona como destino un servidor remoto guardado en la configuración, por id o nombre. Los servidores se agregan en /settings → \"Administrar equipos guardados\" o en Configuración → Servidores de la app de escritorio." : "Targets a remote server saved in settings, by id or name. Servers are added in /settings → \"Manage saved computers\" or in Settings → Servers in the desktop app.",
                Example = "llm-tuner --server gpu-rig --engine ollama --model llama3.1:8b --ctx 16k --yes"
            },
            new()
            {
                Flag = "--host <host>",
                Category = "Server",
                DefaultValue = "0.0.0.0",
                Description = isEs ? "Dirección donde escucha /serve. En /mcp, host personalizado del endpoint del motor." : "Bind address for /serve. With /mcp, a custom engine endpoint host.",
                Example = "llm-tuner /serve --host 0.0.0.0"
            },
            new()
            {
                Flag = "--port <port>",
                Category = "Server",
                DefaultValue = "7860",
                Description = isEs ? "Puerto TCP de /serve." : "TCP port for /serve.",
                Example = "llm-tuner /serve --port 7860"
            },
            new()
            {
                Flag = "--token <token>",
                Category = "Server",
                DefaultValue = "Auto",
                Description = isEs ? "Token Bearer de autenticación de /serve. Si se omite, se genera uno hexadecimal de 32 bytes y se guarda." : "Bearer authentication token for /serve. When omitted, a 32-byte hex token is generated and saved.",
                Example = "llm-tuner /serve --token my-secret-token"
            },
            new()
            {
                Flag = "--name <name>",
                Category = "MCP",
                DefaultValue = "lmstudio / ollama",
                Description = isEs ? "Nombre personalizado del servidor MCP que registra /mcp." : "Custom MCP server name registered by /mcp.",
                Example = "llm-tuner /mcp --engine ollama --name my-local-llm"
            },
            new()
            {
                Flag = "--apiKey <key>",
                Category = "MCP",
                DefaultValue = "sk-lm-local",
                Description = isEs ? "Clave de API personalizada para LM Studio al ejecutar /mcp." : "Custom LM Studio API key when running /mcp.",
                Example = "llm-tuner /mcp --engine lmstudio --apiKey sk-lm-local"
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
                Description = isEs ? "Imprime la versión instalada de llm-tuner y sale (ej: 0.6.0)." : "Prints the installed version of llm-tuner and exits (e.g., 0.6.0).",
                Example = "llm-tuner --version"
            }
        };
    }

    public List<DocSection> GetDocSections()
    {
        bool isEs = _currentLanguage == "es";
        var optionGroupsEs = new Dictionary<string, string>
        {
            ["Engine"] = "Motor",
            ["Model"] = "Modelo",
            ["Configuration"] = "Configuración",
            ["Optimization"] = "Optimización",
            ["Execution"] = "Ejecución",
            ["Automation"] = "Automatización",
            ["Presets"] = "Presets",
            ["Output"] = "Salida",
            ["Chat"] = "Chat",
            ["Server"] = "Servidor",
            ["MCP"] = "MCP",
            ["Settings"] = "Preferencias",
            ["General"] = "General"
        };
        var referenceOptions = GetCommands()
            .Where(c => c.Flag.StartsWith('-'))
            .Select(c => new DocOption
            {
                Flag = c.Flag,
                Description = c.Description,
                DefaultValue = c.DefaultValue == "false" ? "" : c.DefaultValue,
                Group = isEs && optionGroupsEs.TryGetValue(c.Category, out var es) ? es : c.Category
            })
            .ToList();

        return new List<DocSection>
        {
            new()
            {
                Id = "cli-intro",
                Group = "cli",
                Title = isEs ? "Menú interactivo" : "Interactive menu",
                Paragraphs =
                {
                    isEs
                        ? "Ejecuta llm-tuner sin argumentos para abrir un menú persistente con todos los comandos. Muévete con las flechas, elige con Enter y vuelve al paso anterior con Backspace."
                        : "Run llm-tuner without arguments to open a persistent menu with every command. Move with the arrow keys, pick with Enter, and go back one step with Backspace.",
                    isEs
                        ? "Cada comando también se puede ejecutar directo, con o sin la barra: llm-tuner /load y llm-tuner load hacen lo mismo."
                        : "Every command can also be run directly, with or without the slash: llm-tuner /load and llm-tuner load do the same thing.",
                    isEs
                        ? "Requiere Node.js 24 o posterior, y LM Studio (0.4.25+) u Ollama, que también se pueden instalar desde el asistente."
                        : "It requires Node.js 24 or later, and LM Studio (0.4.25+) or Ollama, which can also be installed from the wizard."
                },
                Examples =
                {
                    new() { Caption = isEs ? "Instalar de forma global:" : "Install globally:", Code = "npm install -g llm-tuner" },
                    new() { Caption = isEs ? "O ejecutar sin instalar:" : "Or run it without installing:", Code = "npx llm-tuner" },
                    new() { Caption = isEs ? "Abrir el menú o ir directo a un comando:" : "Open the menu, or go straight to a command:", Code = "llm-tuner\nllm-tuner /load" }
                }
            },
            new()
            {
                Id = "cli-load",
                Group = "cli",
                Title = "/load",
                Paragraphs =
                {
                    isEs
                        ? "Sintoniza y carga un modelo en el equipo activo. El asistente pide el motor, el modelo y el contexto, y el perfil de carga."
                        : "Tunes and loads a model on the active computer. The wizard asks for the engine, the model and context, and the load profile.",
                    isEs
                        ? "Si ya existe un preset para ese modelo, contexto, perfil y hardware, se aplica y el modelo carga en unos 20 segundos. Si no, mide varias configuraciones candidatas (2–3 minutos), guarda la ganadora como preset y carga el modelo."
                        : "If a preset already exists for that model, context, profile, and hardware, it is applied and the model loads in about 20 seconds. Otherwise it measures several candidate configurations (2–3 minutes), saves the winner as a preset, and loads the model.",
                    isEs
                        ? "Si el motor no tiene modelos en este equipo, el asistente pide el nombre de uno para descargarlo. Con un equipo remoto activo, el asistente muestra su nombre y URL y usa sus modelos y presets; las descargas no se hacen en remoto, así que prepara los modelos en ese equipo."
                        : "If the engine has no models on this computer, the wizard asks for one to download. With a remote computer active, the wizard shows its name and URL and uses its models and presets; downloads do not run remotely, so prepare the models on that computer."
                },
                Examples =
                {
                    new() { Code = "llm-tuner /load" }
                },
                Options =
                {
                    new() { Flag = "--engine <lmstudio|ollama>", Description = isEs ? "Motor a usar; si se omite, el asistente pregunta." : "Engine to use; the wizard asks when omitted." },
                    new() { Flag = "--model <key>", Description = isEs ? "Clave del modelo en LM Studio o nombre del modelo en Ollama." : "LM Studio model key or Ollama model name." },
                    new() { Flag = "--ctx <tokens>", Description = isEs ? "Contexto en tokens; acepta 16384 o 16k." : "Context in tokens; accepts 16384 or 16k." },
                    new() { Flag = "--profile <speed|balanced|quality>", Description = isEs ? "Qué optimizar." : "What to optimize for.", DefaultValue = "balanced" },
                    new() { Flag = "--force", Description = isEs ? "Vuelve a medir aunque exista un preset." : "Measures again even if a preset exists." },
                    new() { Flag = "--candidates <n>", Description = isEs ? "Configuraciones a probar al medir; debe ser un número entero positivo." : "Configurations to test when measuring; must be a positive whole number.", DefaultValue = "3" }
                }
            },
            new()
            {
                Id = "cli-chat",
                Group = "cli",
                Title = "/chat",
                Paragraphs =
                {
                    isEs
                        ? "Prueba un modelo cargado con una conversación en el equipo activo. El asistente pide el motor y el modelo; después escribe tus mensajes. El historial se mantiene hasta que escribes /exit."
                        : "Tests a loaded model with a conversation on the active computer. The wizard asks for the engine and the model; then type your messages. History is kept until you type /exit.",
                    isEs
                        ? "Con --prompt envía un solo mensaje, imprime la respuesta y sale. Sin una terminal, por ejemplo en un script, necesita --engine, --model y --prompt."
                        : "With --prompt it sends one message, prints the reply, and exits. Without a terminal, for example in a script, it needs --engine, --model, and --prompt."
                },
                Examples =
                {
                    new() { Code = "llm-tuner /chat" },
                    new() { Caption = isEs ? "Enviar un mensaje y salir:" : "Send one message and exit:", Code = "llm-tuner /chat --engine ollama --model qwen2.5-coder:14b --prompt \"Hello\"" }
                },
                Options =
                {
                    new() { Flag = "--engine <lmstudio|ollama>", Description = isEs ? "Motor con el que conversar." : "Engine to chat with." },
                    new() { Flag = "--model <key>", Description = isEs ? "Modelo a probar." : "Model to test." },
                    new() { Flag = "--prompt <text>", Description = isEs ? "Envía un mensaje y sale." : "Sends one message and exits." },
                    new() { Flag = "--json", Description = isEs ? "Con --prompt, imprime la respuesta completa en JSON." : "With --prompt, prints the full response as JSON." }
                }
            },
            new()
            {
                Id = "cli-mcp",
                Group = "cli",
                Title = "/mcp",
                Paragraphs =
                {
                    isEs
                        ? "Registra LM Studio u Ollama como servidor MCP en tus agentes de IA: Claude Desktop, Claude Code, OpenCode, Antigravity, VS Code y ChatGPT / Codex."
                        : "Registers LM Studio or Ollama as an MCP server in your AI agents: Claude Desktop, Claude Code, OpenCode, Antigravity, VS Code, and ChatGPT / Codex.",
                    isEs
                        ? "El asistente pide el motor y el nombre del servidor, muestra qué agentes detectó en tu equipo y edita la configuración de los que elijas."
                        : "The wizard asks for the engine and the server name, shows which agents it detected on your machine, and edits the configuration of the ones you pick."
                },
                Examples =
                {
                    new() { Code = "llm-tuner /mcp" },
                    new() { Caption = isEs ? "Saltar pasos con banderas:" : "Skip steps with flags:", Code = "llm-tuner /mcp --engine lmstudio --name my-local-llm" }
                },
                Options =
                {
                    new() { Flag = "--engine <lmstudio|ollama>", Description = isEs ? "Motor a registrar." : "Engine to register." },
                    new() { Flag = "--name <name>", Description = isEs ? "Nombre del servidor MCP." : "MCP server name.", DefaultValue = "lmstudio / ollama" },
                    new() { Flag = "--host <host>", Description = isEs ? "Host personalizado del endpoint del motor." : "Custom engine endpoint host." },
                    new() { Flag = "--apiKey <key>", Description = isEs ? "Clave de API de LM Studio." : "LM Studio API key.", DefaultValue = "sk-lm-local" }
                }
            },
            new()
            {
                Id = "cli-presets",
                Group = "cli",
                Title = "/presets",
                Paragraphs =
                {
                    isEs
                        ? "Lista los presets guardados: la configuración ganadora de cada modelo, contexto y perfil en este hardware. Es lo mismo que --presets."
                        : "Lists saved presets: the winning configuration for each model, context, and profile on this hardware. Same as --presets."
                },
                Examples =
                {
                    new() { Code = "llm-tuner /presets" },
                    new() { Caption = isEs ? "Filtrar por motor o modelo:" : "Filter by engine or model:", Code = "llm-tuner --presets --engine ollama" }
                },
                Options =
                {
                    new() { Flag = "--engine <lmstudio|ollama>", Description = isEs ? "Muestra solo los presets de ese motor." : "Shows only that engine's presets." },
                    new() { Flag = "--model <key>", Description = isEs ? "Muestra solo los presets de ese modelo." : "Shows only that model's presets." }
                }
            },
            new()
            {
                Id = "cli-serve",
                Group = "cli",
                Title = "/serve",
                Paragraphs =
                {
                    isEs
                        ? "Comparte este equipo como servidor remoto, para controlarlo desde otra computadora con la CLI (--server) o la app de escritorio. La app de escritorio ofrece lo mismo en Configuración → \"Compartir este equipo\"."
                        : "Shares this computer as a remote server, so you can control it from another computer with the CLI (--server) or the desktop app. The desktop app offers the same under Settings → \"Share this computer\".",
                    isEs
                        ? "Toda petición requiere un token Bearer. Usa el de --token; si no lo pasas, reutiliza el token guardado en server.json, y si no hay ninguno genera uno de 32 bytes y lo guarda. Al arrancar muestra la URL y el token."
                        : "Every request requires a Bearer token. It uses --token; without it, it reuses the token saved in server.json, and if there is none it generates a 32-byte token and saves it. On start it prints the URL and the token.",
                    isEs
                        ? "Desde otra computadora usa la IP de red del servidor con el puerto, no 0.0.0.0 ni localhost. El servidor usa HTTP y el token no cifra el tráfico: úsalo en una red local de confianza."
                        : "From another computer, use the server's LAN IP and port, not 0.0.0.0 or localhost. The server uses HTTP and the token does not encrypt traffic: use it on a trusted LAN.",
                    isEs
                        ? "Para conectar otra PC paso a paso, mira el grupo Servidor remoto."
                        : "To connect another PC step by step, see the Remote server group."
                },
                Examples =
                {
                    new() { Code = "llm-tuner /serve" },
                    new() { Caption = isEs ? "Elegir dirección, puerto y token:" : "Choose the address, port, and token:", Code = "llm-tuner /serve --host 0.0.0.0 --port 7860 --token my-secret-token" }
                },
                Options =
                {
                    new() { Flag = "--host <host>", Description = isEs ? "Dirección donde escucha." : "Address to listen on.", DefaultValue = "0.0.0.0" },
                    new() { Flag = "--port <port>", Description = isEs ? "Puerto TCP." : "TCP port.", DefaultValue = "7860" },
                    new() { Flag = "--token <token>", Description = isEs ? "Token Bearer de autenticación." : "Bearer authentication token.", DefaultValue = isEs ? "guardado o generado" : "saved or generated" }
                }
            },
            new()
            {
                Id = "cli-settings",
                Group = "cli",
                Title = "/settings",
                Paragraphs =
                {
                    isEs
                        ? "Abre el menú de configuración: idioma, tema, inicio automático del servidor al iniciar sesión, datos de conexión del servidor y equipos guardados."
                        : "Opens the settings menu: language, theme, automatic server startup after login, server connection details, and saved computers.",
                    isEs
                        ? "El inicio automático es para tu usuario y empieza en el próximo inicio de sesión, no en el momento. La opción de datos de conexión muestra la dirección guardada y el token; si todavía no hay configuración, ejecuta /serve o activa el inicio automático primero."
                        : "Automatic startup is per user and starts on your next login, not immediately. The connection details option shows the saved address and token; if there is no configuration yet, run /serve or enable automatic startup first.",
                    isEs
                        ? "\"Administrar equipos guardados\" agrega, prueba, selecciona y quita los equipos remotos a los que te conectas. Agregar un equipo no lo conecta: sus motores y modelos se usan al seleccionarlo. El equipo local no se puede quitar."
                        : "\"Manage saved computers\" adds, tests, selects, and removes the remote computers you connect to. Adding a computer does not connect to it: its engines and models are used once you select it. The local computer cannot be removed."
                },
                Examples =
                {
                    new() { Code = "llm-tuner /settings" },
                    new() { Caption = isEs ? "Cambiar idioma o tema sin abrir el menú:" : "Change the language or theme without the menu:", Code = "llm-tuner --lang es\nllm-tuner --theme dark" }
                },
                Options =
                {
                    new() { Flag = "--lang <en|es>", Description = isEs ? "Idioma de la interfaz, guardado para la próxima vez." : "Interface language, saved for next time." },
                    new() { Flag = "--theme <system|light|dark>", Description = isEs ? "Tema de la interfaz, guardado para la próxima vez." : "Interface theme, saved for next time.", DefaultValue = "system" }
                }
            },
            new()
            {
                Id = "cli-help",
                Group = "cli",
                Title = "/help · /exit",
                Paragraphs =
                {
                    isEs
                        ? "/help muestra el uso, las banderas y la carpeta donde se guardan los presets. /exit cierra el menú interactivo."
                        : "/help shows usage, flags, and the folder where presets are saved. /exit closes the interactive menu."
                },
                Examples =
                {
                    new() { Code = "llm-tuner --help\nllm-tuner --version" }
                },
                Options =
                {
                    new() { Flag = "--help, -h", Description = isEs ? "Muestra la ayuda." : "Shows the help." },
                    new() { Flag = "--version, -v", Description = isEs ? "Imprime la versión instalada." : "Prints the installed version." }
                }
            },
            new()
            {
                Id = "cli-automation",
                Group = "cli",
                Title = isEs ? "Automatización" : "Automation",
                Paragraphs =
                {
                    isEs
                        ? "Pasa el motor, el modelo, el contexto y el perfil con --yes para sintonizar y cargar sin preguntas, por ejemplo desde un script o CI."
                        : "Pass the engine, model, context, and profile with --yes to tune and load without prompts, for example from a script or CI.",
                    isEs
                        ? "Los cambios en el servicio de Ollama pueden pedir autorización de administrador en la terminal; el modo JSON o no interactivo nunca pide contraseña."
                        : "Ollama service changes may ask for administrator authorization in the terminal; JSON or noninteractive mode never asks for a password."
                },
                Examples =
                {
                    new() { Caption = isEs ? "Sintonizar y cargar con el perfil balanced:" : "Tune and load with the balanced profile:", Code = "llm-tuner --engine lmstudio --model qwen/qwen2.5-coder-32b --ctx 16k --yes" },
                    new() { Caption = isEs ? "Máxima calidad con salida JSON:" : "Maximum quality with JSON output:", Code = "llm-tuner --engine ollama --model qwen2.5-coder:14b --ctx 32768 --profile quality --yes --json" },
                    new() { Caption = isEs ? "Medir sin aplicar cambios ni guardar preset:" : "Measure without applying changes or saving a preset:", Code = "llm-tuner --engine lmstudio --model qwen/qwen2.5-coder-32b --ctx 16384 --dry-run" },
                    new() { Caption = isEs ? "Usar un servidor remoto guardado:" : "Use a saved remote server:", Code = "llm-tuner --server gpu-rig --engine ollama --model llama3.1:8b --ctx 16384 --yes" }
                },
                Options =
                {
                    new() { Flag = "--yes", Description = isEs ? "No pide confirmación." : "Does not ask for confirmation." },
                    new() { Flag = "--json", Description = isEs ? "Salida final en JSON." : "Final output as JSON." },
                    new() { Flag = "--dry-run", Description = isEs ? "Mide y muestra los cambios sin aplicar nada ni guardar preset." : "Measures and shows the changes without applying anything or saving a preset." },
                    new() { Flag = "--server <id|name>", Description = isEs ? "Usa un servidor remoto guardado; la elección queda guardada para los siguientes comandos." : "Uses a saved remote server; the choice is saved for later commands.", DefaultValue = "local" }
                }
            },
            new()
            {
                Id = "cli-options",
                Group = "cli",
                Title = isEs ? "Referencia de opciones" : "Options reference",
                Paragraphs =
                {
                    isEs ? "Todas las banderas, agrupadas por tema." : "Every flag, grouped by topic."
                },
                Options = referenceOptions
            },
            new()
            {
                Id = "cli-builder",
                Group = "cli",
                Title = isEs ? "Generador de comandos" : "Command builder",
                Paragraphs =
                {
                    isEs
                        ? "Elige motor, modelo, contexto, perfil y servidor para armar un comando y copiarlo."
                        : "Pick an engine, model, context, profile, and server to build a command and copy it."
                }
            },
            new()
            {
                Id = "desktop-launch",
                Group = "desktop",
                Title = isEs ? "Abrir la app" : "Open the app",
                Paragraphs =
                {
                    isEs
                        ? "La app de escritorio guía los mismos pasos que /load: motor, modelo y contexto, y carga. La barra lateral muestra tu CPU, la RAM y cada GPU con su VRAM y enlace PCIe. Ofrece las mismas capacidades que la CLI."
                        : "The desktop app walks through the same steps as /load: engine, model and context, and load. The side rail shows your CPU, RAM, and each GPU with its VRAM and PCIe link. It offers the same capabilities as the CLI.",
                    isEs
                        ? "La primera vez descarga el runtime de Electron, así que necesita conexión. Si no arranca, la CLI funciona sin él. En Linux también registra un acceso directo en ~/.local/share/applications/llm-tuner.desktop."
                        : "The first launch downloads the Electron runtime, so it needs a network connection. If it cannot start, the CLI works without it. On Linux it also registers a launcher at ~/.local/share/applications/llm-tuner.desktop."
                },
                Examples =
                {
                    new() { Code = "llm-tuner-desktop" }
                }
            },
            new()
            {
                Id = "desktop-engine",
                Group = "desktop",
                Title = isEs ? "Elegir motor" : "Choose an engine",
                Paragraphs =
                {
                    isEs
                        ? "Cada tarjeta muestra si LM Studio u Ollama está instalado y su versión. La recomendada lleva una etiqueta y el motivo; por ejemplo, con varias GPU se recomienda LM Studio porque permite fijar el orden de las GPU y el tipo de KV por modelo."
                        : "Each card shows whether LM Studio or Ollama is installed and its version. The recommended one has a badge and the reason; for example, with several GPUs LM Studio is recommended because it lets you set the GPU order and the KV type per model.",
                    isEs
                        ? "Si un motor no está instalado, la app ofrece instalarlo y elegir el método de instalación."
                        : "If an engine is not installed, the app offers to install it and lets you choose the install method."
                }
            },
            new()
            {
                Id = "desktop-model",
                Group = "desktop",
                Title = isEs ? "Modelo y contexto" : "Model and context",
                Paragraphs =
                {
                    isEs
                        ? "Elige uno de tus modelos descargados, el contexto en tokens (512 como mínimo, con atajos rápidos) y el perfil de carga: speed, balanced o quality."
                        : "Pick one of your downloaded models, the context in tokens (512 minimum, with quick chips), and the load profile: speed, balanced, or quality.",
                    isEs
                        ? "Marca \"Volver a medir aunque exista un preset\" para forzar una medición nueva, igual que --force."
                        : "Check \"Measure again even if a preset exists\" to force a new measurement, like --force.",
                    isEs
                        ? "\"Configuraciones a medir\" equivale a --candidates, con un número entero positivo (3 si lo dejas vacío). \"Prueba: medir y ver los cambios sin aplicarlos\" equivale a --dry-run."
                        : "\"Configurations to measure\" matches --candidates, with a positive whole number (3 when left empty). \"Dry run: measure and preview without applying\" matches --dry-run.",
                    isEs
                        ? "Si el motor no tiene modelos en este equipo, \"Descargar un primer modelo\" pide el nombre del modelo y muestra el progreso. Con un servidor remoto activo no aparece: las descargas solo se hacen en el equipo local, así que prepara los modelos en ese equipo."
                        : "If the engine has no models on this computer, \"Download a first model\" asks for the model name and shows the progress. With a remote server active it does not appear: downloads run only on the local computer, so prepare the models on that computer."
                }
            },
            new()
            {
                Id = "desktop-load",
                Group = "desktop",
                Title = isEs ? "Carga y benchmark" : "Load and benchmark",
                Paragraphs =
                {
                    isEs
                        ? "Con un preset existente, el modelo carga en unos 20 segundos. Si no hay preset para ese contexto y perfil, la app prueba varias configuraciones con tu hardware; tarda unos minutos y solo ocurre una vez."
                        : "With an existing preset, the model loads in about 20 seconds. If there is no preset for that context and profile, the app tests several configurations on your hardware; it takes a few minutes and only happens once.",
                    isEs
                        ? "Mientras mide, una tabla muestra cada configuración con su estimación, capas en GPU, VRAM y velocidad con contexto corto y profundo, junto a la telemetría de CPU, RAM y VRAM en vivo."
                        : "While it measures, a table shows each configuration with its estimate, GPU layers, VRAM, and short and deep context speed, next to live CPU, RAM, and VRAM telemetry.",
                    isEs
                        ? "Antes de cambiar algo, \"¿Aplicar estos cambios?\" lista los cambios. \"Aplicar y cargar\" los aplica y carga el modelo; \"No aplicar\" no aplica nada ni carga el modelo, y las mediciones ya guardadas como preset siguen disponibles. Sin respuesta en 2 minutos, los cambios no se aplican."
                        : "Before changing anything, \"Apply these changes?\" lists the changes. \"Apply and load\" applies them and loads the model; \"Do not apply\" applies nothing and does not load the model, and measurements already saved as a preset stay available. With no answer within 2 minutes, the changes are not applied.",
                    isEs
                        ? "En una prueba (dry run) no aparece esa pregunta y el resultado dice \"Solo vista previa\": no se aplicó, cargó ni guardó ningún preset."
                        : "A dry run skips that question and the result says \"Preview only\": nothing was applied, loaded, or saved as a preset."
                }
            },
            new()
            {
                Id = "desktop-chat",
                Group = "desktop",
                Title = isEs ? "Probar el modelo" : "Test the model",
                Paragraphs =
                {
                    isEs
                        ? "Cuando el modelo termina de cargar, el panel \"Probar conexión\" te deja enviar un mensaje para comprobar que responde."
                        : "When the model finishes loading, the \"Test connection\" panel lets you send a prompt to check that it responds."
                }
            },
            new()
            {
                Id = "desktop-presets",
                Group = "desktop",
                Title = isEs ? "Presets guardados" : "Saved presets",
                Paragraphs =
                {
                    isEs
                        ? "El botón \"Presets guardados\" abre un panel con los presets, igual que /presets: modelo, motor, contexto, perfil, tipo de KV, descarga en GPU, velocidad y fecha. Se pueden filtrar por motor y por modelo."
                        : "The \"Saved presets\" button opens a panel with your presets, like /presets: model, engine, context, profile, KV type, GPU offload, speed, and date. You can filter them by engine and by model."
                }
            },
            new()
            {
                Id = "desktop-mcp",
                Group = "desktop",
                Title = isEs ? "Agregar como MCP" : "Add as MCP",
                Paragraphs =
                {
                    isEs
                        ? "El botón \"Agregar como MCP\" debajo de cada motor abre el mismo registro que /mcp: el nombre del servidor MCP, opciones avanzadas de conexión (URL base y clave de API) y la lista de agentes."
                        : "The \"Add as MCP\" button under each engine opens the same registration as /mcp: the MCP server name, advanced connection options (base URL and API key), and the list of agents.",
                    isEs
                        ? "Los agentes detectados en tu equipo vienen marcados. \"Guardar y configurar\" edita la configuración de cada agente elegido."
                        : "Agents detected on your machine come checked. \"Save & Configure\" edits the configuration of each selected agent."
                }
            },
            new()
            {
                Id = "desktop-settings",
                Group = "desktop",
                Title = isEs ? "Configuración" : "Settings",
                Paragraphs =
                {
                    isEs
                        ? "El botón Configuración abre un panel con el idioma (English / Español) y el tema (sistema, claro u oscuro)."
                        : "The Settings button opens a panel with the language (English / Español) and the theme (system, light, or dark).",
                    isEs
                        ? "En \"Configuración del servidor local\" puedes activar \"Iniciar servidor después del inicio de sesión\", que se aplica en el próximo inicio de sesión, y ver o copiar el token de autenticación con \"Mostrar token\" y \"Copiar token\". Mantén el token en privado."
                        : "Under \"Local Server Settings\" you can enable \"Start server after login\", which applies on your next login, and view or copy the authentication token with \"Reveal token\" and \"Copy token\". Keep the token private.",
                    isEs
                        ? "\"Compartir este equipo\" inicia el mismo servidor que /serve: revisa el host y el puerto, escribe un token si quieres (es opcional) y pulsa \"Empezar a compartir\". El estado muestra la dirección para los demás equipos. El servidor se detiene al cerrar la app, y hay que cerrarla para cambiar sus ajustes."
                        : "\"Share this computer\" starts the same server as /serve: check the host and port, enter a token if you want one (it is optional), and press \"Start sharing\". The status shows the address for other computers. The server stops when you close the app, and you must close the app to change its settings."
                }
            },
            new()
            {
                Id = "desktop-servers",
                Group = "desktop",
                Title = isEs ? "Servidores remotos" : "Remote servers",
                Paragraphs =
                {
                    isEs
                        ? "En Configuración → Servidores, \"Agregar servidor\" pide un nombre, la URL del servidor (por ejemplo http://192.168.1.100:7860) y el token. \"Probar conexión\" lo verifica antes de guardarlo. Guardarlo no lo activa: elígelo en la lista para usarlo."
                        : "Under Settings → Servers, \"Add Server\" asks for a name, the server URL (for example http://192.168.1.100:7860), and the token. \"Test Connection\" checks it before you save it. Saving does not activate it: choose it in the list to use it.",
                    isEs
                        ? "Al elegir un servidor en la lista, el hardware, los modelos, los presets, la carga y el chat pasan a ese servidor; la configuración de la app, las actualizaciones y la de los agentes siguen siendo locales. Con un servidor activo, \"Probar conexión\" lo verifica y \"Eliminar\" lo borra de la lista después de confirmar."
                        : "Picking a server in the list switches hardware, models, presets, loading, and chat to that server; app settings, updates, and agent configuration stay local. With a server active, \"Test connection\" checks it and \"Remove\" deletes it from the list after you confirm.",
                    isEs
                        ? "Para levantar el servidor y conectarte paso a paso, mira el grupo Servidor remoto."
                        : "For step-by-step server setup and connection, see the Remote server group."
                }
            },
            new()
            {
                Id = "remote-overview",
                Group = "remote",
                Title = isEs ? "Cómo funciona" : "How it works",
                Paragraphs =
                {
                    isEs
                        ? "Una PC con GPU (el servidor) comparte su equipo con /serve o con \"Compartir este equipo\" en la app de escritorio. Otra computadora (el cliente) lo controla por la red local con la app de escritorio o la CLI."
                        : "A GPU PC (the server) shares itself with /serve or with \"Share this computer\" in the desktop app. Another computer (the client) controls it over the local network with the desktop app or the CLI.",
                    isEs
                        ? "Con un servidor remoto activo, el hardware, los modelos, los presets, la carga y el chat pasan a ese servidor. La configuración de la app, las actualizaciones y la configuración MCP de los agentes siguen siendo del cliente. Instalar motores y descargar modelos solo se puede hacer en la PC servidor."
                        : "With a remote server active, hardware, models, presets, loading, and chat move to that server. App settings, updates, and the agents' MCP configuration stay on the client. Installing engines and downloading models can only be done on the server PC itself.",
                    isEs
                        ? "El servidor usa HTTP: el token autentica pero no cifra el tráfico. Úsalo solo en una red local de confianza, nunca expuesto a internet. Los equipos guardados aceptan URL HTTP o HTTPS; prefiere HTTPS en redes no confiables."
                        : "The server uses HTTP: the token authenticates but does not encrypt traffic. Use it only on a trusted local network, never exposed to the internet. Saved computers accept HTTP or HTTPS URLs; prefer HTTPS on untrusted networks."
                }
            },
            new()
            {
                Id = "remote-start",
                Group = "remote",
                Title = isEs ? "1. Levantar el servidor (PC con GPU)" : "1. Start the server (GPU PC)",
                Paragraphs =
                {
                    isEs
                        ? "En la PC con GPU, inicia el modo servidor. Escucha en 0.0.0.0:7860 y muestra la URL con la IP de red de la PC y el token. Si no pasas --token, reutiliza el guardado en server.json o genera uno nuevo y lo guarda. En la app de escritorio, usa Configuración → \"Compartir este equipo\"; ese servidor se detiene al cerrar la app."
                        : "On the GPU PC, start server mode. It listens on 0.0.0.0:7860 and prints the URL with the PC's network IP and the token. Without --token it reuses the one saved in server.json, or generates a new one and saves it. In the desktop app, use Settings → \"Share this computer\" instead; that server stops when you close the app.",
                    isEs
                        ? "Para que arranque solo, activa \"Inicio automático del servidor después de iniciar sesión\" en /settings, o \"Iniciar servidor después del inicio de sesión\" en la configuración de la app. Se aplica desde el próximo inicio de sesión de tu usuario y, si falta server.json, lo crea con 0.0.0.0:7860 y un token. Queda registrado en ~/.config/autostart en Linux, en ~/Library/LaunchAgents en macOS y en la carpeta Inicio de Windows."
                        : "To start it automatically, enable \"Automatic server startup after login\" in /settings, or \"Start server after login\" in the app settings. It applies from your user's next login and, if server.json is missing, creates it with 0.0.0.0:7860 and a token. It is registered in ~/.config/autostart on Linux, ~/Library/LaunchAgents on macOS, and the Startup folder on Windows.",
                    isEs
                        ? "El firewall de la PC servidor debe permitir conexiones entrantes TCP en el puerto del servidor (7860 por defecto). Esto depende de tu sistema, no de llm-tuner."
                        : "The server PC's firewall must allow inbound TCP connections on the server port (7860 by default). This depends on your system, not on llm-tuner."
                },
                Examples =
                {
                    new() { Code = "llm-tuner /serve" },
                    new() { Caption = isEs ? "Al arrancar muestra algo así:" : "On start it prints something like:", Code = "LLM Tuner server mode running on http://192.168.1.100:7860\nAuthentication Token: 3f9c…" },
                    new() { Caption = isEs ? "Con puerto y token propios:" : "With your own port and token:", Code = "llm-tuner /serve --port 7860 --token my-secret-token" }
                },
                Options =
                {
                    new() { Flag = "--host <host>", Description = isEs ? "Dirección donde escucha." : "Address to listen on.", DefaultValue = "0.0.0.0" },
                    new() { Flag = "--port <port>", Description = isEs ? "Puerto TCP." : "TCP port.", DefaultValue = "7860" },
                    new() { Flag = "--token <token>", Description = isEs ? "Token Bearer de autenticación." : "Bearer authentication token.", DefaultValue = isEs ? "guardado o generado" : "saved or generated" }
                }
            },
            new()
            {
                Id = "remote-details",
                Group = "remote",
                Title = isEs ? "2. Obtener la dirección y el token" : "2. Get the address and token",
                Paragraphs =
                {
                    isEs
                        ? "En la PC servidor, la CLI los muestra en /settings → \"Ver detalles de conexión del servidor y token\". En la app de escritorio están en Configuración → \"Configuración del servidor local\", con \"Mostrar token\" y \"Copiar token\"."
                        : "On the server PC, the CLI shows them in /settings → \"View server connection details and token\". In the desktop app they are under Settings → \"Local Server Settings\", with \"Reveal token\" and \"Copy token\".",
                    isEs
                        ? "Desde la otra computadora usa la IP de red de la PC servidor con el puerto, por ejemplo http://192.168.1.100:7860. No sirven 0.0.0.0 ni localhost. Mantén el token en privado."
                        : "From the other computer, use the server PC's network IP and port, for example http://192.168.1.100:7860. Neither 0.0.0.0 nor localhost works. Keep the token private."
                },
                Examples =
                {
                    new() { Code = "llm-tuner /settings" }
                }
            },
            new()
            {
                Id = "remote-desktop",
                Group = "remote",
                Title = isEs ? "3. Conectarse desde la app de escritorio" : "3. Connect from the desktop app",
                Paragraphs =
                {
                    isEs
                        ? "En la computadora cliente abre Configuración → Servidores → \"Agregar servidor\". Completa \"Nombre del servidor\", \"URL del servidor\" (http://192.168.1.100:7860) y \"Token de autenticación\", usa \"Probar conexión\" y después \"Guardar servidor\"."
                        : "On the client computer, open Settings → Servers → \"Add Server\". Fill in \"Server Name\", \"Server URL\" (http://192.168.1.100:7860), and \"Authentication Token\", use \"Test Connection\", then \"Save Server\".",
                    isEs
                        ? "Guardarlo no lo activa: elige el servidor en la lista de Servidores para usarlo. Con un servidor activo, \"Eliminar\" lo borra después de confirmar y vuelve a esta PC (local)."
                        : "Saving does not activate it: choose the server in the Servers list to use it. With a server active, \"Remove\" deletes it after you confirm and returns to this PC (local).",
                    isEs
                        ? "La URL del servidor no se puede abrir en un navegador: cada petición, incluida la página, exige el token. Conéctate siempre desde la app o la CLI."
                        : "The server URL cannot be opened in a browser: every request, the page included, requires the token. Always connect from the app or the CLI."
                },
                Examples =
                {
                    new() { Code = "llm-tuner-desktop" }
                }
            },
            new()
            {
                Id = "remote-cli",
                Group = "remote",
                Title = isEs ? "4. Conectarse desde la CLI" : "4. Connect from the CLI",
                Paragraphs =
                {
                    isEs
                        ? "En la computadora cliente abre /settings → \"Administrar equipos guardados\" → \"Agregar un equipo\" y escribe el nombre, la URL y el token. Usa \"Probar conexión\" para verificarlo. La CLI y la app comparten la misma configuración (settings.json), así que un servidor agregado en una aparece en la otra."
                        : "On the client computer, open /settings → \"Manage saved computers\" → \"Add a computer\" and enter the name, URL, and token. Use \"Test connection\" to check it. The CLI and the app share the same configuration (settings.json), so a server added in one appears in the other.",
                    isEs
                        ? "Agregarlo no lo conecta. Elígelo con \"Usar equipo\" en ese menú, o con --server y su nombre o id. La elección queda guardada para los siguientes comandos y también la usa la app; elegir un servidor en la app cambia el de la CLI. /load, /chat y /presets trabajan sobre el servidor activo. Usa --server local para volver a esta PC."
                        : "Adding it does not connect to it. Pick it with \"Use computer\" in that menu, or with --server and its name or id. The choice is saved for later commands and the app uses it too; picking a server in the app changes the CLI's. /load, /chat, and /presets work on the active server. Use --server local to return to this PC."
                },
                Examples =
                {
                    new() { Caption = isEs ? "Agregar y seleccionar el equipo:" : "Add and select the computer:", Code = "llm-tuner /settings" },
                    new() { Caption = isEs ? "Cargar un modelo en el servidor remoto:" : "Load a model on the remote server:", Code = "llm-tuner --server gpu-rig --engine ollama --model llama3.1:8b --ctx 16384 --yes" },
                    new() { Caption = isEs ? "Ver los presets del servidor activo:" : "List the active server's presets:", Code = "llm-tuner /presets" },
                    new() { Caption = isEs ? "Volver a esta PC:" : "Return to this PC:", Code = "llm-tuner --server local /presets" }
                }
            },
            new()
            {
                Id = "remote-troubleshooting",
                Group = "remote",
                Title = isEs ? "Solución de problemas" : "Troubleshooting",
                Paragraphs =
                {
                    isEs ? "Mensajes que puedes ver y cómo resolverlos:" : "Messages you may see and how to fix them:"
                },
                Options =
                {
                    new()
                    {
                        Flag = isEs ? "No autorizado: se requiere un token Bearer válido." : "Unauthorized: a valid Bearer token is required.",
                        Description = isEs ? "El token no coincide. Cópialo de nuevo en la PC servidor y actualiza el servidor en la app." : "The token does not match. Copy it again on the server PC and update the server in the app."
                    },
                    new()
                    {
                        Flag = isEs ? "No se pudo conectar al servidor" : "Could not connect to server",
                        Description = isEs ? "El servidor no está corriendo, la IP o el puerto son incorrectos, o el firewall bloquea el puerto." : "The server is not running, the IP or port is wrong, or the firewall blocks the port."
                    },
                    new()
                    {
                        Flag = isEs ? "Prohibido: esta operación no se puede ejecutar de forma remota." : "Forbidden: this operation cannot be performed remotely.",
                        Description = isEs ? "Instalar motores y descargar modelos no está permitido en remoto. Hazlo directamente en la PC servidor." : "Installing engines and downloading models is not allowed remotely. Do it directly on the server PC."
                    },
                    new()
                    {
                        Flag = isEs ? "No se pudo iniciar el servidor compartido en {host}:{port}. Quizá otro programa usa ese puerto." : "Could not start the shared server on {host}:{port}. Another program may be using that port.",
                        Description = isEs ? "Elige otro puerto o cierra el programa que lo usa." : "Choose another port or close the program using it."
                    },
                    new()
                    {
                        Flag = isEs ? "El servidor compartido ya está en marcha con otros ajustes. Cierra la app para cambiarlos." : "The shared server is already running with other settings. Close the app to change them.",
                        Description = isEs ? "Cierra la app de escritorio y vuelve a abrirla para compartir con otro host, puerto o token." : "Close the desktop app and open it again to share with another host, port, or token."
                    },
                    new()
                    {
                        Flag = isEs ? "No hay modelos disponibles en el equipo activo para {engine}." : "No models are available on the active computer for {engine}.",
                        Description = isEs ? "Instala o descarga un modelo en ese equipo e inténtalo de nuevo." : "Install or download a model on that computer, then try again."
                    },
                    new()
                    {
                        Flag = isEs ? "Aún no hay configuración del servidor." : "No server configuration yet.",
                        Description = isEs ? "En la PC servidor, ejecuta /serve o activa el inicio automático para crear server.json." : "On the server PC, run /serve or enable automatic startup to create server.json."
                    },
                    new()
                    {
                        Flag = "Unknown server",
                        Description = isEs ? "El nombre o id de --server no existe en la configuración. Agrégalo en /settings o en la app, o revisa cómo se escribe." : "The --server name or id is not in the settings. Add it in /settings or the app, or check the spelling."
                    }
                }
            }
        };
    }

    public List<DocSection> GetShowcaseSections()
    {
        bool isEs = _currentLanguage == "es";
        return new List<DocSection>
        {
            new()
            {
                Id = "cli-menu",
                Group = "cli",
                Title = isEs ? "Menú de comandos" : "Command menu",
                Paragraphs =
                {
                    isEs
                        ? "Sin argumentos, llm-tuner abre un menú persistente con /load, /chat, /mcp, /presets, /serve, /settings, /help y /exit. Cada comando también se puede ejecutar directo, por ejemplo llm-tuner /load."
                        : "Without arguments, llm-tuner opens a persistent menu with /load, /chat, /mcp, /presets, /serve, /settings, /help, and /exit. Every command can also be run directly, for example llm-tuner /load."
                },
                Examples = { new() { Code = "llm-tuner" } },
                ImagePath = "screenshots/cli-01-menu.png",
                ImageAlt = isEs ? "Menú de comandos de llm-tuner en la terminal" : "llm-tuner command menu in the terminal"
            },
            new()
            {
                Id = "cli-wizard",
                Group = "cli",
                Title = isEs ? "Asistente de carga" : "Load wizard",
                Paragraphs =
                {
                    isEs
                        ? "Asistente guiado paso a paso: detecta CPU, RAM y GPUs, muestra los motores instalados, lista los modelos descargados y estima el contexto máximo que entra completo en GPU."
                        : "Guided step-by-step wizard: it inspects CPU, RAM, and GPUs, shows the installed engines, lists downloaded models, and estimates the largest context that fits fully on GPU."
                },
                Examples = { new() { Code = "llm-tuner /load" } },
                ImagePath = "screenshots/cli-01-wizard.png",
                ImageAlt = isEs ? "Asistente /load en la terminal" : "/load wizard in the terminal"
            },
            new()
            {
                Id = "cli-headless",
                Group = "cli",
                Title = isEs ? "Modo no interactivo" : "Non-interactive mode",
                Paragraphs =
                {
                    isEs
                        ? "Ejecución sin preguntas para scripts de inicio o pipelines: aplica la configuración en LM Studio y carga Qwen3 Coder 30B con contexto de 32K."
                        : "Prompt-free execution for startup scripts or pipelines: it applies the configuration to LM Studio and loads Qwen3 Coder 30B with a 32K context."
                },
                Examples = { new() { Code = "llm-tuner --engine lmstudio --model qwen/qwen3-coder-30b --ctx 32768 --profile balanced --yes" } },
                ImagePath = "screenshots/cli-02-headless.png",
                ImageAlt = isEs ? "Carga no interactiva en la terminal" : "Non-interactive load in the terminal"
            },
            new()
            {
                Id = "cli-mcp",
                Group = "cli",
                Title = isEs ? "Integración MCP" : "MCP integration",
                Paragraphs =
                {
                    isEs
                        ? "Elige el motor (LM Studio u Ollama), nombra el servidor MCP y selecciona los agentes de IA detectados. Backspace vuelve al paso anterior en cada pantalla."
                        : "Pick the engine (LM Studio or Ollama), name the MCP server, and select the detected AI agents. Backspace goes back one step on every screen."
                },
                Examples = { new() { Code = "llm-tuner /mcp" } },
                ImagePath = "screenshots/cli-06-mcp.png",
                ImageAlt = isEs ? "Primer paso de /mcp en la terminal" : "First step of /mcp in the terminal"
            },
            new()
            {
                Id = "cli-presets",
                Group = "cli",
                Title = isEs ? "Presets guardados" : "Saved presets",
                Paragraphs =
                {
                    isEs
                        ? "Muestra las configuraciones ganadoras guardadas (en Linux, en ~/.config/llm-tuner/presets/): velocidad en tokens/s, contexto, tipo de KV cache y reparto exacto de capas entre las GPUs."
                        : "Shows the saved winning configurations (on Linux, in ~/.config/llm-tuner/presets/): speed in tokens/s, context, KV cache type, and the exact layer split across GPUs."
                },
                Examples = { new() { Code = "llm-tuner /presets" } },
                ImagePath = "screenshots/cli-03-presets.png",
                ImageAlt = isEs ? "Lista de presets en la terminal" : "Preset list in the terminal"
            },
            new()
            {
                Id = "cli-settings",
                Group = "cli",
                Title = isEs ? "Configuración" : "Settings",
                Paragraphs =
                {
                    isEs
                        ? "Menú para cambiar el idioma (English / Español) y el tema (oscuro, claro o del sistema), guardados en settings.json. Desde la versión 0.5.0 también incluye el inicio automático del servidor y sus datos de conexión, y desde la 0.6.0 los equipos guardados; esta captura todavía no los muestra."
                        : "Menu to change the language (English / Spanish) and the theme (dark, light, or system), saved in settings.json. Since version 0.5.0 it also includes automatic server startup and its connection details, and since 0.6.0 saved computers; this screenshot does not show them yet."
                },
                Examples = { new() { Code = "llm-tuner /settings" } },
                ImagePath = "screenshots/cli-04-settings.png",
                ImageAlt = isEs ? "Menú /settings en la terminal" : "/settings menu in the terminal"
            },
            new()
            {
                Id = "cli-help",
                Group = "cli",
                Title = isEs ? "Ayuda" : "Help",
                Paragraphs =
                {
                    isEs
                        ? "Lista de comandos y referencia de banderas: perfiles de carga, candidatos a medir (--candidates), ejecución en seco (--dry-run), un solo mensaje con /chat (--prompt), salida JSON y --force, además de notas sobre los equipos guardados, el inicio automático del servidor y la autorización del servicio de Ollama."
                        : "Command list and flag reference: load profiles, candidates to measure (--candidates), dry runs (--dry-run), a single /chat message (--prompt), JSON output, and --force, plus notes on saved computers, automatic server startup, and Ollama service authorization."
                },
                Examples = { new() { Code = "llm-tuner --help" } },
                ImagePath = "screenshots/cli-05-help.png",
                ImageAlt = isEs ? "Salida de llm-tuner --help" : "llm-tuner --help output"
            },
            new()
            {
                Id = "desktop-engine",
                Group = "desktop",
                Title = isEs ? "Hardware y elección de motor" : "Hardware and engine selection",
                Paragraphs =
                {
                    isEs
                        ? "La app detecta el CPU (Ryzen 7 5800X), la RAM (60.7 GB) y las dos GPUs (2x RTX 5070 de 12 GB) con su enlace PCIe. Aquí recomienda LM Studio porque es el único instalado; con varias GPUs también lo recomienda, porque permite fijar el orden de las GPU y el tipo de KV por modelo. Cada motor ofrece el botón \"Agregar como MCP\"."
                        : "The app detects the CPU (Ryzen 7 5800X), RAM (60.7 GB), and both GPUs (2x RTX 5070 12 GB) with their PCIe link. Here it recommends LM Studio because it is the only one installed; with several GPUs it also recommends it, because it lets you set the GPU order and the KV type per model. Every engine offers an \"Add as MCP\" button."
                },
                Examples = { new() { Code = "llm-tuner-desktop" } },
                ImagePath = "screenshots/01-engine-selection.png",
                ImageAlt = isEs ? "Pantalla de elección de motor en la app de escritorio" : "Engine selection screen in the desktop app"
            },
            new()
            {
                Id = "desktop-model",
                Group = "desktop",
                Title = isEs ? "Modelo, contexto y perfil" : "Model, context, and profile",
                Paragraphs =
                {
                    isEs
                        ? "Qwen3 Coder 30B (MoE 8/128, 48 capas, Q4_K_M) con contexto de 32K y el perfil balanced. La app encuentra el preset guardado para este hardware, medido antes en 177.1 t/s. Desde la versión 0.6.0 esta pantalla también tiene el botón \"Presets guardados\", \"Configuraciones a medir\" y la opción de prueba (dry run), que esta captura todavía no muestra."
                        : "Qwen3 Coder 30B (MoE 8/128, 48 layers, Q4_K_M) with a 32K context and the balanced profile. The app finds the preset saved for this hardware, previously measured at 177.1 t/s. Since version 0.6.0 this screen also has the \"Saved presets\" button, \"Configurations to measure\", and the dry run option, which this screenshot does not show yet."
                },
                ImagePath = "screenshots/02-model-selection.png",
                ImageAlt = isEs ? "Selección de modelo, contexto y perfil" : "Model, context, and profile selection"
            },
            new()
            {
                Id = "desktop-loaded",
                Group = "desktop",
                Title = isEs ? "Carga desde el preset" : "Load from the preset",
                Paragraphs =
                {
                    isEs
                        ? "Con el preset existente, la carga terminó en unos 15 segundos sin volver a medir. Las 48 capas quedaron en VRAM con KV cache f16, a 145.8 tokens/s."
                        : "With the existing preset, loading finished in about 15 seconds without measuring again. All 48 layers fit in VRAM with an f16 KV cache, at 145.8 tokens/s."
                },
                ImagePath = "screenshots/03-model-loaded.png",
                ImageAlt = isEs ? "Modelo cargado desde el preset" : "Model loaded from the preset"
            },
            new()
            {
                Id = "desktop-chat",
                Group = "desktop",
                Title = isEs ? "Probar el modelo" : "Test the model",
                Paragraphs =
                {
                    isEs
                        ? "El panel \"Probar conexión\" envía mensajes al endpoint local recién cargado. Aquí Qwen3 Coder 30B genera código Python (Fibonacci con memoización)."
                        : "The \"Test connection\" panel sends prompts to the freshly loaded local endpoint. Here Qwen3 Coder 30B generates Python code (Fibonacci with memoization)."
                },
                ImagePath = "screenshots/04-chat-benchmark.png",
                ImageAlt = isEs ? "Panel de prueba de conexión con una respuesta del modelo" : "Connection test panel with a model response"
            },
            new()
            {
                Id = "desktop-settings",
                Group = "desktop",
                Title = isEs ? "Configuración" : "Settings",
                Paragraphs =
                {
                    isEs
                        ? "Panel lateral para cambiar el idioma (English / Español) y el tema (oscuro, claro o del sistema), guardados en ~/.config/llm-tuner/settings.json. Desde la versión 0.5.0 también incluye los servidores remotos y la configuración del servidor local, y desde la 0.6.0 \"Compartir este equipo\"; esta captura todavía no los muestra."
                        : "Side panel to change the language (English / Spanish) and the theme (dark, light, or system), saved in ~/.config/llm-tuner/settings.json. Since version 0.5.0 it also includes remote servers and local server settings, and since 0.6.0 \"Share this computer\"; this screenshot does not show them yet."
                },
                ImagePath = "screenshots/05-settings-drawer.png",
                ImageAlt = isEs ? "Panel de configuración de la app de escritorio" : "Desktop app settings panel"
            },
            new()
            {
                Id = "desktop-mcp",
                Group = "desktop",
                Title = isEs ? "Agregar como servidor MCP" : "Add as an MCP server",
                Paragraphs =
                {
                    isEs
                        ? "Registra LM Studio u Ollama como servidor MCP: nombre personalizado, URL base y clave de API opcionales, y la lista de agentes (Claude Desktop, Claude Code, OpenCode, Antigravity, VS Code, ChatGPT / Codex) que marca cuáles están detectados."
                        : "Registers LM Studio or Ollama as an MCP server: custom name, optional base URL and API key, and the list of agents (Claude Desktop, Claude Code, OpenCode, Antigravity, VS Code, ChatGPT / Codex) showing which are detected."
                },
                ImagePath = "screenshots/06-mcp-modal.png",
                ImageAlt = isEs ? "Ventana para agregar un motor como servidor MCP" : "Dialog to add an engine as an MCP server"
            }
        };
    }

    public List<DocSection> GetProfileSections()
    {
        bool isEs = _currentLanguage == "es";
        return new List<DocSection>
        {
            new()
            {
                Id = "profiles-intro",
                Group = "profiles",
                Title = isEs ? "Cómo funcionan los perfiles" : "How profiles work",
                Paragraphs =
                {
                    isEs
                        ? "Más tokens por segundo no siempre significa mejores respuestas. El perfil de carga decide cuándo se puede cambiar calidad de respuesta por velocidad o por memoria."
                        : "More tokens per second does not always mean better answers. The load profile decides when answer quality can be traded for speed or memory.",
                    isEs
                        ? "Hay tres perfiles: speed, balanced (por defecto) y quality. El asistente recomienda el de mayor calidad que entra completo en la VRAM libre. Las optimizaciones sin pérdida (reparto de capas entre GPUs, orden de las GPU, hilos de CPU) se aplican en los tres."
                        : "There are three profiles: speed, balanced (the default), and quality. The wizard recommends the highest-quality one that fits fully in free VRAM. Lossless optimizations (layer split across GPUs, GPU order, CPU threads) apply to all three."
                },
                Examples = { new() { Code = "llm-tuner --engine lmstudio --model qwen/qwen2.5-coder-32b --ctx 16k --profile balanced --yes" } }
            },
            new()
            {
                Id = "profiles-speed",
                Group = "profiles",
                Title = "speed",
                Paragraphs =
                {
                    isEs
                        ? "Busca el máximo de tokens por segundo. Usa la variante descargada más liviana del modelo y permite cualquier tipo de KV cache, incluido q4_0."
                        : "Goes for the most tokens per second. It uses the lightest downloaded variant of the model and allows any KV cache type, q4_0 included.",
                    isEs
                        ? "A cambio, pierde precisión en las respuestas. Sirve para modelos pesados donde cada token por segundo cuenta o para procesar mucho volumen."
                        : "In exchange, answers lose precision. It suits heavy models where every token per second counts, or bulk processing."
                },
                Examples = { new() { Code = "llm-tuner --engine lmstudio --model qwen/qwen2.5-coder-32b --ctx 16k --profile speed --yes" } },
                Options =
                {
                    new() { Flag = isEs ? "Variante" : "Variant", Description = isEs ? "La más liviana descargada." : "The lightest downloaded one." },
                    new() { Flag = "KV cache", Description = "f16, q8_0, q4_0" }
                }
            },
            new()
            {
                Id = "profiles-balanced",
                Group = "profiles",
                Title = isEs ? "balanced (por defecto)" : "balanced (default)",
                Paragraphs =
                {
                    isEs
                        ? "Velocidad ponderada por la calidad del KV cache. Usa la variante que elegiste y ordena las configuraciones candidatas por tokens/s × calidad del KV."
                        : "Speed weighted by KV cache quality. It uses the variant you selected and ranks candidate configurations by tokens/s × KV quality.",
                    isEs
                        ? "Es el punto medio para el uso diario: ni el más agresivo ni el más pesado."
                        : "It is the middle ground for everyday use: neither the most aggressive nor the heaviest."
                },
                Examples = { new() { Code = "llm-tuner --engine lmstudio --model qwen/qwen2.5-coder-32b --ctx 16k --profile balanced --yes" } },
                Options =
                {
                    new() { Flag = isEs ? "Variante" : "Variant", Description = isEs ? "La que elegiste." : "The one you selected." },
                    new() { Flag = "KV cache", Description = isEs ? "f16, q8_0, q4_0, ponderados por calidad" : "f16, q8_0, q4_0, weighted by quality" }
                }
            },
            new()
            {
                Id = "profiles-quality",
                Group = "profiles",
                Title = "quality",
                Paragraphs =
                {
                    isEs
                        ? "La menor pérdida de calidad que todavía corre completa en GPU. Elige la variante descargada más pesada que entra al 100% en VRAM, con un KV cache preciso (f16 o q8_0)."
                        : "The least quality loss that still runs fully on GPU. It picks the heaviest downloaded variant that fits 100% in VRAM, with a precise KV cache (f16 or q8_0).",
                    isEs
                        ? "Si ninguna configuración entra completa en GPU, vuelve a balanced. Rinde mejor con VRAM de sobra, por ejemplo varias GPUs o tarjetas de 16 GB o más."
                        : "If no configuration fits fully on GPU, it falls back to balanced. It works best with VRAM to spare, for example several GPUs or 16 GB+ cards."
                },
                Examples = { new() { Code = "llm-tuner --engine ollama --model qwen2.5-coder:14b --ctx 32768 --profile quality --yes" } },
                Options =
                {
                    new() { Flag = isEs ? "Variante" : "Variant", Description = isEs ? "La más pesada que entra completa en VRAM." : "The heaviest that fits fully in VRAM." },
                    new() { Flag = "KV cache", Description = "f16, q8_0" }
                }
            },
            new()
            {
                Id = "profiles-variants",
                Group = "profiles",
                Title = isEs ? "Cambio de variante" : "Variant switching",
                Paragraphs =
                {
                    isEs
                        ? "Ollama: elige y carga automáticamente la etiqueta (tag) más adecuada para el perfil entre las que reconoce como el mismo modelo."
                        : "Ollama: it automatically picks and loads the best tag for the profile among the tags it recognizes as the same model.",
                    isEs
                        ? "LM Studio: optimiza la variante activa y te avisa si otra cuantización descargada (o descargable, como Q8_0, Q6_K o Q5_K_M) se ajusta mejor al perfil."
                        : "LM Studio: it optimizes the active variant and tells you if another downloaded (or downloadable, such as Q8_0, Q6_K, or Q5_K_M) quantization better fits the profile."
                }
            },
            new()
            {
                Id = "profiles-remeasure",
                Group = "profiles",
                Title = isEs ? "Cuándo vuelve a medir" : "When it measures again",
                Paragraphs =
                {
                    isEs
                        ? "Cada preset se guarda por modelo, contexto, perfil y huella de hardware. Solo se vuelve a medir si cambia el contexto o el perfil, el hardware (GPU, VRAM, enlace PCIe), el archivo del modelo o, para speed y quality, las variantes descargadas."
                        : "Each preset is saved per model, context, profile, and hardware fingerprint. It only measures again if the context or profile changes, the hardware changes (GPU, VRAM, PCIe link), the model file changes, or, for speed and quality, the downloaded variants change.",
                    isEs
                        ? "Para forzar una medición nueva, usa --force."
                        : "To force a new measurement, use --force."
                },
                Examples = { new() { Code = "llm-tuner --engine ollama --model qwen2.5-coder:14b --ctx 32768 --profile quality --force --yes" } }
            },
            new()
            {
                Id = "profiles-comparison",
                Group = "profiles",
                Title = isEs ? "Comparación" : "Comparison",
                Paragraphs =
                {
                    isEs ? "Los tres perfiles lado a lado." : "The three profiles side by side."
                }
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
                    ? "Sin argumentos abre un menú persistente con /load, /chat, /mcp, /presets, /serve, /settings, /help y /exit. Backspace vuelve al paso anterior y /load detecta motores, lista modelos por peso, calcula capas GPU y muestra la carga en vivo como en la app de escritorio."
                    : "Without arguments it opens a persistent menu with /load, /chat, /mcp, /presets, /serve, /settings, /help, and /exit. Backspace steps back, and /load detects engines, sorts models by size, calculates GPU layers, and shows a desktop-like live loading animation.",
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
                Id = "serve",
                Name = isEs ? "Modo Servidor Remoto" : "Remote Server Mode",
                Description = isEs
                    ? "Ejecuta llm-tuner como servicio de red en una máquina con GPU dedicada, con autenticación Bearer de tiempo constante y telemetría saneada. Puede iniciarse solo al iniciar sesión, y otros clientes CLI (--server) o de escritorio (Configuración → Servidores) lo controlan desde la red."
                    : "Runs llm-tuner as a network service on a dedicated GPU machine, with timing-safe Bearer authentication and sanitized telemetry. It can start automatically after login, and other CLI (--server) or desktop (Settings → Servers) clients control it over the network.",
                Command = "llm-tuner /serve",
                BinTarget = "src/cli/commands/serve.js",
                Badge = "Server"
            },
            new()
            {
                Id = "settings",
                Name = isEs ? "Menú de Configuración" : "Interactive Settings Menu",
                Description = isEs
                    ? "Configuración rápida de preferencias globales del usuario: cambia el idioma de la aplicación (EN / ES) o el tema visual (System, Light, Dark), activa el inicio automático del servidor al iniciar sesión, consulta la dirección y el token de conexión y administra los equipos guardados."
                    : "Terminal preference manager: switch application language (English / Español) and visual theme (System, Light, Dark), enable automatic server startup after login, view the server address and connection token, and manage saved computers.",
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
