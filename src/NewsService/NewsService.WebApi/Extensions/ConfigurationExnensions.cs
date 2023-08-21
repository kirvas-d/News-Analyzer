using EfCoreRepository.NewsRepository;
using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Core.Services;
using NewsService.Core.Abstractions;
using NewsService.Core.Models;
using RabbitMqService.Models;

namespace NewsAnalyzer.Application.NewsService.Extensions;

public static class ConfigurationExtensions
{
    public static void AddServicesConfiguration(this IServiceCollection services, IConfiguration configuration) 
    {
        var rssUrls = new List<string>() { "https://lenta.ru/rss" };
        var parsers = new List<IHtmlParser>() { new LentaHtmlParser() };
        var rssConfiguration = new RssNewsLoaderConfiguration(rssUrls, parsers);
        services.AddSingleton(sp => rssConfiguration);

        var rssNewsServiceConfiruration = new BackgroundRssNewsServiceConfiguration 
        { 
            ScaningIntervalTime = TimeSpan.FromDays(1) 
        };
        services.AddSingleton(sp => rssNewsServiceConfiruration);

        var rabbitMqMessengerServiceConfiguration = new RabbitMqMessengerServiceConfiguration
        {
            HostName = configuration["RabbitMq:HostName"],
            ExcangeName = configuration["RabbitMq:ExchangeName"],
            QueueName = configuration["RabbitMq:QueueName"]
        };
        services.AddSingleton(sp => rabbitMqMessengerServiceConfiguration);

        services.AddDbContext<NewsDbContext>(options => options.UseNpgsql(configuration["NewsDb:ConnectionString"]), ServiceLifetime.Singleton);
    }
}
