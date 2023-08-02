using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Core.Abstractions;
using NewsAnalyzer.Core.Models;
using NewsAnalyzer.Core.Services;
using NewsAnalyzer.Infrastructure.EfCoreRepository.NewsRepository;
using NewsAnalyzer.Infrastructure.RabbitMqService.Models;

namespace NewsAnalyzer.Application.NewsService.Extensions;

public static class ConfigurationExnensions
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
            QueueName = configuration["RabbitMq:QueueName"]
        };
        services.AddSingleton(sp => rabbitMqMessengerServiceConfiguration);

        services.AddDbContext<NewsDbContext>(options => options.UseNpgsql(configuration["NewsDb:ConnectionString"]), ServiceLifetime.Singleton);
    }
}
