using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using llm_tuner_web_site;
using llm_tuner_web_site.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<ClipboardService>();
builder.Services.AddScoped<DocumentationDataService>();

await builder.Build().RunAsync();
