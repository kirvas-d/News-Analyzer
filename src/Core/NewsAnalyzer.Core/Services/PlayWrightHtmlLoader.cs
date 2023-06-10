using Microsoft.Playwright;
using NewsAnalyzer.Core.Abstractions;

namespace NewsAnalyzer.Core.Services;

public class PlayWrightHtmlLoader : IHtmlLoader
{
    private readonly IPlaywright _playwright;
    private readonly IBrowser _browser;

    public PlayWrightHtmlLoader()
    {
        _playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
        _browser = _playwright.Chromium.LaunchAsync().GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        _browser.DisposeAsync().GetAwaiter().GetResult();
        _playwright.Dispose();
    }

    public async Task<string> GetHtmlBodyAsync(string uri)
    {
        var page = await _browser.NewPageAsync();
        await page.GotoAsync(uri, new PageGotoOptions() { Timeout = 60000 });
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions() { Timeout = 60000 });
        var htmlContent = await page.ContentAsync();
        await page.CloseAsync();

        return htmlContent;
    }
}
