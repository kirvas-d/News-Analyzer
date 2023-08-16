using NewsAnalyzer.Application.NerService.Extensions;
using NewsAnalyzer.Application.NerService.Services;
using NewsAnalyzer.Core.Abstractions;
using NewsAnalyzer.Core.Events;
using NewsAnalyzer.Core.NerService.Services;
using NewsAnalyzer.Infrastructure.EfCoreRepository.NamedEntityFormRepository;
using NewsAnalyzer.Infrastructure.RabbitMqService.Abstractions;
using NewsAnalyzer.Infrastructure.RabbitMqService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddHostedService<BackgroundNerService>();
builder.Services.AddSingleton<INerService, CatalystNerService>();
builder.Services.AddSingleton<IMessengerConsumerService<NewsLoadedEventArgs>, RabbitMqMessengerConsumerService<NewsLoadedEventArgs>>();
builder.Services.AddSingleton<INamedEntityFormRepository, NamedEntityFormEfCoreRepository>();
builder.Services.AddServicesConfiguration(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
