namespace NewsService.Core.Tests;

using NewsService.Core.HtmlLoader.Services;

public class PlayWrightHtmlLoaderTest : HtmlLoaderServiceTest, IDisposable
{
    public PlayWrightHtmlLoaderTest() : base(new PlayWrightHtmlLoader())
    {
    }

    public void Dispose()
    {
        ((PlayWrightHtmlLoader)_htmlLoaderService).Dispose();
    }
}
