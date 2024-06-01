namespace NewsService.Core.NewsLoader.Services;

using System.ServiceModel.Syndication;
using System.Xml;
using NewsService.Core.HtmlLoader.Abstracts;
using NewsService.Core.HtmlParsers.Abstracts;
using NewsService.Core.NewsLoader.Abstracts;
using NewsService.Core.NewsLoader.Models;

public abstract class RssNewsLoaderBase : INewsLoader
{
    private readonly IHtmlLoader _htmlLoader;
    private readonly IHtmlParser _htmlParser;
    private readonly string _rssUrl;

    public string RssUrl => _rssUrl;

    protected RssNewsLoaderBase(IHtmlLoader htmlLoader, IHtmlParser htmlParser, string rssUrl)
    {
        _htmlLoader = htmlLoader;
        _htmlParser = htmlParser;
        _rssUrl = rssUrl;
    }

    public IEnumerable<NewsInfo> GetNewsInfos()
    {
        using var xmlReader = XmlReader.Create(_rssUrl);
        var feed = SyndicationFeed.Load(xmlReader);

        foreach (var item in feed.Items)
        {
            var links = item.Links.Where(link => !link.Uri.ToString().StartsWith("https://icdn"));
            foreach (var link in links)
            {
                yield return new NewsInfo(
                    _rssUrl,
                    link.Uri.ToString(),
                    item.Title.Text,
                    DateTime.SpecifyKind(item.PublishDate.DateTime, DateTimeKind.Utc));
            }
        }
    }

    public async IAsyncEnumerable<News> LoadNewsAsync(IEnumerable<NewsInfo> newsInfos)
    {
        foreach (var newsInfo in newsInfos)
        {
            var htmlBody = await _htmlLoader.GetHtmlBodyAsync(newsInfo.SourceName);
            var text = await _htmlParser.GetTextFromBody(htmlBody);

            yield return new News(
                Guid.NewGuid(),
                newsInfo.SourceName,
                newsInfo.Title,
                text,
                newsInfo.PublishDate);
        }
    }
}
