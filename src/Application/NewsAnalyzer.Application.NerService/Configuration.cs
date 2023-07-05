﻿using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Infrastructure.EfCoreRepository;
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

        services.AddDbContext<NamedEntityDbContext>(options => options.UseNpgsql("Host=192.168.0.171;Port=5432;Database=named_entity_db;Username=homeserver;Password=Pechorin"), ServiceLifetime.Singleton);
    }
}
