using Microsoft.JSInterop;

namespace llm_tuner_web_site.Services;

public class ClipboardService
{
    private readonly IJSRuntime _js;

    public ClipboardService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<bool> CopyAsync(string text)
    {
        try
        {
            return await _js.InvokeAsync<bool>("llmTuner.copyToClipboard", text);
        }
        catch
        {
            return false;
        }
    }
}
