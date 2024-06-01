namespace NlpService.WebApi.Extensions;

using MassTransit;
using Microsoft.EntityFrameworkCore;
using NewsService.Core.NewsLoader.Events;
using NlpService.Data;
using NlpService.WebApi.Services;
using System.Reflection;

public static class ConfigurationExtensions
{
    public static void AddServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NamedEntityDbContext>(options => options.UseNpgsql(configuration["NamedEntityDb:ConnectionString"]), ServiceLifetime.Scoped);

        services.AddMassTransit(x =>
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            x.AddConsumer<NewsLoadedEventArgsConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:HostName"]);
                cfg.ReceiveEndpoint(configuration["RabbitMq:QueueName"], e =>
                {
                    e.Bind<NewsLoadedEventArgs>();
                    e.ConcurrentMessageLimit = 1;
                    e.ConfigureConsumer<NewsLoadedEventArgsConsumer>(context);
                });
            });
        });
    }
}
