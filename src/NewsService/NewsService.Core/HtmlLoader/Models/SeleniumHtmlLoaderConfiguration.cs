namespace NewsService.Core.HtmlLoader.Models;

public enum BrowserType 
{
    Chrome,
    Edge,
}

public class SeleniumHtmlLoaderConfiguration
{
    public const string SeleniumHtmlLoaderConfigurationKey = nameof(SeleniumHtmlLoaderConfiguration);

    public BrowserType BrowserType { get; init; }

    public required Uri Uri { get; init; }
}
