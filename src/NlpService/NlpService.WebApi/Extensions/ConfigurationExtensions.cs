using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using NlpService.Repository.NamedEntityFormRepository;
using NlpService.SentimentAnalyzeService.Models;
using RabbitMqService.Models;
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
        services.AddDbContext<NamedEntityFormDbContext>(options => options.UseNpgsql(configuration["NamedEntityDb:ConnectionString"]), ServiceLifetime.Singleton);
        services.AddSingleton(sp => new ApplicationNewsClient(GrpcChannel.ForAddress(configuration["ApplicationNewsHost"])));
        services.AddSingleton(sp => new MlSentimentAnalyzeServiceConfiguration { ModelFilePath = configuration["MlSentimentAnalyzeModelPath"]});
    }
}
