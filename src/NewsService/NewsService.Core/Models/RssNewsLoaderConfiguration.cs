using NewsService.Core.Abstractions;

namespace NewsService.Core.Models;

public class RssNewsLoaderConfiguration
{
    private readonly List<(string, IHtmlParser)> _listOfParsing;

    public IEnumerable<(string, IHtmlParser)> ListOfParsing => _listOfParsing;

    public RssNewsLoaderConfiguration(IEnumerable<string> rssUrls, IEnumerable<IHtmlParser> htmlParsers)
    {
        //RssUrls = rssUrls;
        //HtmlParsers = htmlParsers;
        _listOfParsing = new List<(string, IHtmlParser)>();
        foreach (var rssUrl in rssUrls)
        {
            var parser = htmlParsers.FirstOrDefault(p => rssUrl.StartsWith(p.SiteUrl));
            if (parser != null)
            {
                _listOfParsing.Add((rssUrl, parser));

            }
            else
            {
                throw new Exception($"Not find parser for {rssUrl}");
            }
        }
    }
}
