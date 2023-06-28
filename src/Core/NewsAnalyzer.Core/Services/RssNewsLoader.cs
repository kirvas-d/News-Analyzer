using AngleSharp.Dom;
using NewsAnalyzer.Core.Abstractions;
using NewsAnalyzer.Core.Models;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsAnalyzer.Core.Services;

public class RssNewsLoader : INewsLoader
{
    private readonly RssNewsLoaderConfiguration _configuration;
    private readonly IHtmlLoader _htmlLoader;
    public RssNewsLoader(RssNewsLoaderConfiguration configuration, IHtmlLoader htmlLoader)
    {
        _configuration = configuration;
        _htmlLoader = htmlLoader;
    }

    public async IAsyncEnumerable<News> LoadNewsAsync()
    {
        foreach (var (rssUrl, parser) in _configuration.ListOfParsing) 
        {
            await foreach (var news in LoadNewsFromRss(rssUrl, parser)) 
            {
                yield return news;
            }
        }
    }

    private async IAsyncEnumerable<News> LoadNewsFromRss(string rssUrl, IHtmlParser parser) 
    {
        using var xmlReader = XmlReader.Create(rssUrl);
        var feed = SyndicationFeed.Load(xmlReader);

        foreach (var item in feed.Items)
        {
            var links = item.Links.Where(link => !link.Uri.ToString().StartsWith("https://icdn"));
            foreach (var link in links)
            {
                var htmlBody = await _htmlLoader.GetHtmlBodyAsync(link.Uri.ToString());
                var text = await parser.GetTextFromBody(htmlBody);

                yield return new News(Guid.NewGuid(), 
                                      link.Uri.ToString(),
                                      item.Title.Text,
                                      text,
                                      DateTime.SpecifyKind(item.PublishDate.DateTime, DateTimeKind.Utc));
            }
        }
    }
}
