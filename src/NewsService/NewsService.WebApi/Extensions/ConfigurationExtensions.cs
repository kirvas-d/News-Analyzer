using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewsService.Core.HtmlLoader.Models;
using NewsService.Core.NewsLoader.Abstracts;
using NewsService.Core.NewsLoader.Services;
using NewsService.Core.Services;
using NewsService.Repository.NewsRepository;

namespace NewsService.WebApi.Extensions;

public static class ConfigurationExtensions
{
    public static void AddServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var rssNewsServiceConfiguration = new BackgroundNewsServiceConfiguration(TimeSpan.FromDays(1));
        services.AddSingleton(sp => rssNewsServiceConfiguration);
        services.AddSingleton<INewsLoader, LentaRssNewsLoader>();
        services.AddSingleton<SeleniumHtmlLoaderConfiguration>(configuration
            .GetSection(SeleniumHtmlLoaderConfiguration.SeleniumHtmlLoaderConfigurationKey)
            .Get<SeleniumHtmlLoaderConfiguration>());

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:HostName"]);
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddDbContext<NewsDbContext>(options => options.UseNpgsql(configuration["NewsDb:ConnectionString"]), ServiceLifetime.Singleton);
    }
}
