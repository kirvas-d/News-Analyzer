namespace NewsService.Core.HtmlParsers;

using AngleSharp;
using NewsService.Core.HtmlParsers.Abstracts;

public class LentaHtmlParser : IHtmlParser
{
    public string SiteUrl => "https://lenta.ru";

    public async Task<string> GetTextFromBody(string htmlBody)
    {
        var context = BrowsingContext.New();
        var document = await context.OpenAsync(request => request.Content(htmlBody));
        var contentElements = document.QuerySelectorAll("div.topic-body__content p");
        var result = "";
        foreach (var contentElement in contentElements)
        {
            result += contentElement.TextContent + "\n";
        }

        return result;
    }
}
