namespace NewsService.Core.HtmlParsers.Abstracts;

public interface IHtmlParser
{
    string SiteUrl { get; }

    Task<string> GetTextFromBody(string htmlBody);
}
