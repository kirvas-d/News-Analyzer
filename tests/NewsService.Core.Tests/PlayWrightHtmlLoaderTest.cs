using NewsService.Core.HtmlLoader.Services;

namespace NewsService.Core.Tests;

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
