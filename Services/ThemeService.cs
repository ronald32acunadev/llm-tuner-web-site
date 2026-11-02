using Microsoft.JSInterop;

namespace llm_tuner_web_site.Services;

public class ThemeService
{
    private readonly IJSRuntime _js;
    private string _currentTheme = "dark";

    public string CurrentTheme => _currentTheme;
    public event Action? OnThemeChanged;

    public ThemeService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task InitializeAsync()
    {
        try
        {
            var saved = await _js.InvokeAsync<string>("llmTuner.getTheme");
            if (!string.IsNullOrEmpty(saved))
            {
                _currentTheme = saved;
                OnThemeChanged?.Invoke();
            }
        }
        catch
        {
            _currentTheme = "dark";
        }
    }

    public async Task SetThemeAsync(string theme)
    {
        _currentTheme = theme;
        try
        {
            await _js.InvokeVoidAsync("llmTuner.setTheme", theme);
        }
        catch { }
        OnThemeChanged?.Invoke();
    }
}
