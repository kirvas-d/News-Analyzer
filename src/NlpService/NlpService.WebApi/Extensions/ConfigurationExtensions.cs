using Grpc.Net.Client;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NewsService.Core.NewsLoader.Events;
using NlpService.Data;
using NlpService.WebApi.Services;
using System.Reflection;
using static NewsAnalyzer.Application.NewsService.ApplicationNews;

namespace NlpService.WebApi.Extensions;

public static class ConfigurationExtensions
{
    public static void AddServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NamedEntityDbContext>(options => options.UseNpgsql(configuration["NamedEntityDb:ConnectionString"]), ServiceLifetime.Scoped);
        services.AddSingleton(sp => new ApplicationNewsClient(GrpcChannel.ForAddress(configuration["ApplicationNewsHost"])));

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
