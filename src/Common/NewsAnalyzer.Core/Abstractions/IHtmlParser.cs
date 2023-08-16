namespace NewsAnalyzer.Core.Abstractions;

public interface IHtmlParser
{
    string SiteUrl { get; }

    Task<string> GetTextFromBody(string htmlBody);
}
