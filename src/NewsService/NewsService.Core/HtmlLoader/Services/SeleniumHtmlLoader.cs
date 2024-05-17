using NewsService.Core.HtmlLoader.Abstracts;
using NewsService.Core.HtmlLoader.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;

namespace NewsService.Core.HtmlLoader.Services;

public class SeleniumHtmlLoader : IHtmlLoader
{
    private readonly IWebDriver _webDriver;

    public SeleniumHtmlLoader(SeleniumHtmlLoaderConfiguration configuration) 
    {
        var options = GetDriverOptions(configuration.BrowserType);
        _webDriver = new RemoteWebDriver(configuration.Uri, options);
    }

    public Task<string> GetHtmlBodyAsync(string uri)
    {
        _webDriver.Navigate().GoToUrl(uri);
        return Task.FromResult(_webDriver.PageSource);
    }

    private DriverOptions GetDriverOptions(BrowserType browserType) => browserType switch
    {
        BrowserType.Chrome => new ChromeOptions(),
        BrowserType.Edge => new EdgeOptions(),
        _ => throw new NotImplementedException(),
    };
}
