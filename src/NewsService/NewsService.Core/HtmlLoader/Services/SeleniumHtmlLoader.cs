using NewsService.Core.HtmlLoader.Abstracts;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NewsService.Core.HtmlLoader.Services;

public class SeleniumHtmlLoader : IHtmlLoader
{
    private readonly IWebDriver _webDriver;

    public SeleniumHtmlLoader() 
    {
        _webDriver = new ChromeDriver();
    }

    public Task<string> GetHtmlBodyAsync(string uri)
    {
        _webDriver.Navigate().GoToUrl(uri);
        return Task.FromResult(_webDriver.PageSource);
    }
}
