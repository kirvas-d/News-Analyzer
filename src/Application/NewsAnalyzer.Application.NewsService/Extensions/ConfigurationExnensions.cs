using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Core.Abstractions;
using NewsAnalyzer.Core.Models;
using NewsAnalyzer.Core.Services;
using NewsAnalyzer.Infrastructure.EfCoreRepository.NewsRepository;
using NewsAnalyzer.Infrastructure.RabbitMqService.Models;

namespace NewsAnalyzer.Application.NewsService.Extensions;

public static class ConfigurationExnensions
{
    public static void AddServicesConfiguration(this IServiceCollection services) 
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

        services.AddDbContext<NewsDbContext>(options => options.UseNpgsql("Host=192.168.0.171;Port=5432;Database=news_db;Username=homeserver;Password=Pechorin"), ServiceLifetime.Singleton);
    }
}
