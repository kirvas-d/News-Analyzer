using NewsAnalyzer.Core.Abstractions;
using NewsAnalyzer.Core.Models;
using NewsAnalyzer.Core.Services;
using NewsAnalyzer.Infrastructure.RabbitMqService.Models;
using System.Reflection;

namespace NewsAnalyzer.Application.NewsService.Extensions;

public static class ConfigurationExnensions
{
    public static void AddRssSourceNewsConfiguration(this IServiceCollection services) 
    {
        var rssUrls = new List<string>() { "https://lenta.ru/rss" };
        var parsers = new List<IHtmlParser>() { new LentaHtmlParser() };
        var configuration = new RssNewsLoaderConfiguration(rssUrls, parsers);
        services.AddSingleton(sp => configuration);

        var rssNewsServiceConfiruration = new BackgroundRssNewsServiceConfiguration 
        { 
            ScaningIntervalTime = TimeSpan.FromDays(1) 
        };
        services.AddSingleton(sp => rssNewsServiceConfiruration);

        var rabbitMqMessengerServiceConfiguration = new RabbitMqMessengerServiceConfiguration
        {
            HostName = "192.168.0.171",
            QueueName = $"NewsService.NewsLoaded"
        };
        services.AddSingleton(sp => rabbitMqMessengerServiceConfiguration);
    }
}
