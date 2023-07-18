using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Infrastructure.EfCoreRepository.NamedEntityFormRepository;
using NewsAnalyzer.Infrastructure.RabbitMqService.Models;

namespace NewsAnalyzer.Application.NerService;

public static class Configuration
{
    public static void AddServicesConfiguration(this IServiceCollection services)
    {
        var rabbitMqMessengerServiceConfiguration = new RabbitMqMessengerServiceConfiguration
        {
            HostName = "192.168.0.171",
            QueueName = $"NewsService.NewsLoaded"
        };
        services.AddSingleton(sp => rabbitMqMessengerServiceConfiguration);

        services.AddDbContext<NamedEntityFormDbContext>(options => options.UseNpgsql("Host=192.168.0.171;Port=5432;Database=named_entity_form_db;Username=homeserver;Password=Pechorin"), ServiceLifetime.Singleton);
    }
}
