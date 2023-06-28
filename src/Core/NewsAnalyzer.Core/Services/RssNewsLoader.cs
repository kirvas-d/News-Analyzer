using AngleSharp.Common;
using NewsAnalyzer.Core.Abstractions;
using NewsAnalyzer.Core.Models;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsAnalyzer.Core.Services;

public class RssNewsLoader : INewsLoader
{
    private readonly RssNewsLoaderConfiguration _configuration;
    private readonly IHtmlLoader _htmlLoader;
    private readonly Dictionary<string, IHtmlParser> _htmlParserDictionary;
    public RssNewsLoader(RssNewsLoaderConfiguration configuration, IHtmlLoader htmlLoader)
    {
        _configuration = configuration;
        _htmlLoader = htmlLoader;
        _htmlParserDictionary = _configuration.ListOfParsing.ToDictionary(item => item.Item1, item => item.Item2);
    }

    public IEnumerable<NewsInfo> GetNewsInfos()
    {
        foreach (var (rssUrl, parser) in _configuration.ListOfParsing)
        {
            using var xmlReader = XmlReader.Create(rssUrl);
            var feed = SyndicationFeed.Load(xmlReader);

            foreach (var item in feed.Items)
            {
                var links = item.Links.Where(link => !link.Uri.ToString().StartsWith("https://icdn"));
                foreach (var link in links)
                {
                    yield return new NewsInfo(rssUrl,
                                              link.Uri.ToString(), 
                                              item.Title.Text, 
                                              DateTime.SpecifyKind(item.PublishDate.DateTime, DateTimeKind.Utc));
                }
            }
        }
    }

    public async IAsyncEnumerable<News> LoadNewsAsync(IEnumerable<NewsInfo> newsInfos)
    {
        foreach (var newsInfo in newsInfos) 
        {
            var htmlBody = await _htmlLoader.GetHtmlBodyAsync(newsInfo.SourceName);
            var parser = _htmlParserDictionary[newsInfo.RssUrl];
            var text = await parser.GetTextFromBody(htmlBody);

            yield return new News(Guid.NewGuid(),
                                  newsInfo.SourceName,
                                  newsInfo.Title,
                                  text,
                                  newsInfo.PublishDate);
        }
    }
}
