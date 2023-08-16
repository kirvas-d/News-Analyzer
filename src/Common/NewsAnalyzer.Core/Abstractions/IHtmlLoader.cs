namespace NewsAnalyzer.Core.Abstractions;

public interface IHtmlLoader
{
    Task<string> GetHtmlBodyAsync(string uri);
}
