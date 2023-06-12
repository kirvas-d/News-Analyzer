using AngleSharp.Dom;
using NewsAnalyzer.Core.Abstractions;
using NewsAnalyzer.Core.Models;
using System;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsAnalyzer.Core.Services;

public class RssSourceNewsLoader : ISourceNewsLoader
{
    private readonly RssSourceNewsLoaderConfiguration _configuration;
    private readonly IEnumerable<IHtmlParser> _parsers;
    private readonly IHtmlLoader _htmlLoader;
    private readonly Dictionary<string, IHtmlParser> _parserDictionary;
    public RssSourceNewsLoader(RssSourceNewsLoaderConfiguration configuration, IEnumerable<IHtmlParser> parsers, IHtmlLoader htmlLoader)
    {
        _configuration = configuration;
        _parsers = parsers;
        _htmlLoader = htmlLoader;
        _parserDictionary = new Dictionary<string, IHtmlParser>();
        foreach (var rssUrl in _configuration.RssUrls) 
        {
            var parser = _parsers.FirstOrDefault(p => rssUrl.StartsWith(p.SiteUrl));
            if (parser != null)
            {
                throw new Exception($"Not find parser for {rssUrl}");
            }
            else
            {
                _parserDictionary.Add(rssUrl, parser);
            }
        }
    }

    public async IAsyncEnumerable<News> LoadNewsAsync()
    {
        foreach (var rssUrl in _configuration.RssUrls) 
        {
            await foreach (var news in LoadNewsFromRss(rssUrl)) 
            {
                yield return news;
            }
        }
    }

    private async IAsyncEnumerable<News> LoadNewsFromRss(string rssUrl) 
    {
        using var xmlReader = XmlReader.Create(rssUrl);
        var feed = SyndicationFeed.Load(xmlReader);

        foreach (var item in feed.Items)
        {
            var links = item.Links.Where(link => !link.Uri.ToString().StartsWith("https://icdn"));
            foreach (var link in links)
            {
                var htmlBody = await _htmlLoader.GetHtmlBodyAsync(link.Uri.ToString());
                var text = await _parserDictionary[rssUrl].GetTextFromBody(htmlBody);

                yield return new News(_parserDictionary[rssUrl].SiteUrl,
                                      item.Title.Text,
                                      text,
                                      item.PublishDate.DateTime);
            }
        }
    }
}
