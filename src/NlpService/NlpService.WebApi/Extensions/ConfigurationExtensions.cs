using Grpc.Net.Client;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NewsService.Core.Events;
using NlpService.Data;
using NlpService.SentimentAnalyzeService.Models;
using NlpService.WebApi.Services;
using RabbitMqService.Models;
using System.Reflection;
using static NewsAnalyzer.Application.NewsService.ApplicationNews;

namespace NlpService.WebApi.Extensions;

public static class ConfigurationExtensions
{
    public static void AddServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqMessengerServiceConfiguration = new RabbitMqMessengerServiceConfiguration
        {
            HostName = configuration["RabbitMq:HostName"],
            ExcangeName = configuration["RabbitMq:ExchangeName"],
            QueueName = configuration["RabbitMq:QueueName"]
        };
        services.AddSingleton(sp => rabbitMqMessengerServiceConfiguration);
        services.AddDbContext<NamedEntityDbContext>(options => options.UseNpgsql(configuration["NamedEntityDb:ConnectionString"]), ServiceLifetime.Scoped);
        services.AddSingleton(sp => new ApplicationNewsClient(GrpcChannel.ForAddress(configuration["ApplicationNewsHost"])));
        services.AddSingleton(sp => new MlSentimentAnalyzeServiceConfiguration 
        { 
            ModelFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), configuration["MlSentimentAnalyzeModelPath"])
        });

        services.AddMassTransit(x =>
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            x.AddConsumers(entryAssembly);
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("192.168.0.171");

                cfg.ReceiveEndpoint("nlp-service", e => 
                {
                    e.ConcurrentMessageLimit = 1;
                    e.ConfigureConsumer<NewsLoadedEventArgsConsumer>(context);                
                });

                //cfg.ConfigureEndpoints(context);
            });
        });
    }
}
