namespace NewsService.Core.HtmlLoader.Abstracts;

public interface IHtmlLoader
{
    Task<string> GetHtmlBodyAsync(string uri);
}
