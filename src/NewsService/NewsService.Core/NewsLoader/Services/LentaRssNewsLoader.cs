using NewsService.Core.HtmlLoader.Abstracts;
using NewsService.Core.HtmlParsers;

namespace NewsService.Core.NewsLoader.Services;

public sealed class LentaRssNewsLoader : RssNewsLoaderBase
{
    public LentaRssNewsLoader(IHtmlLoader htmlLoader)
        : base(htmlLoader, new LentaHtmlParser(), "https://lenta.ru/rss")
    {
    }
}
