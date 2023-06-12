using NewsAnalyzer.Core.Services;

namespace NewsAnalyzer.Core.Tests;

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
