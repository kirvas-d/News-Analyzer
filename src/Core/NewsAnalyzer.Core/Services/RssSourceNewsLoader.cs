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

    public IAsyncEnumerable<string> LoadTexts()
    {
        foreach (var keyValue in _parserDictionary) 
        {
            using var xmlReader = XmlReader.Create(keyValue.Key);
            var feed = SyndicationFeed.Load(xmlReader);

            foreach (var item in feed.Items)
            {
                Console.WriteLine($"Text: {item.Title.Text}");
                Console.WriteLine($"Type: {item.Title.Type}");
                foreach (var link in item.Links.Where(link => !link.Uri.ToString().Contains("https:\\icdn")))
                {
                    Console.WriteLine($"    Type: {link.MediaType}");
                    Console.WriteLine($"    Link: {link.Uri}");
                    var parser = new LentaParser(htmlLoader, link.Uri.ToString());
                    var text = parser.GetEntitys().FirstOrDefault();
                }
                Console.WriteLine(item.PublishDate);
                Console.WriteLine();
            }
        }
    }
}
