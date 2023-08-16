using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Infrastructure.EfCoreRepository.NamedEntityFormRepository;
using NewsAnalyzer.Infrastructure.RabbitMqService.Models;
using static NewsAnalyzer.Application.NewsService.ApplicationNews;

namespace NewsAnalyzer.Application.NerService.Extensions;

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
    }
}
