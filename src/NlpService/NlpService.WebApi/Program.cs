using EfCoreRepository.NamedEntityFormRepository;
using NewsAnalyzer.Core.Abstractions;
using NewsService.Core.Events;
using NlpService.Core.Abstractions;
using NlpService.NerService.Services;
using NlpService.SentimentAnalyzeService.Services;
using NlpService.WebApi.Extensions;
using NlpService.WebApi.Services;
using RabbitMqService.Abstractions;
using RabbitMqService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddHostedService<BackgroundNlpService>();
builder.Services.AddSingleton<INerService, CatalystNerService>();
builder.Services.AddSingleton<ISentimentAnalyzeService, MlSentimentAnalyzeService>();
builder.Services.AddSingleton<IMessengerConsumerService<NewsLoadedEventArgs>, RabbitMqMessengerConsumerService<NewsLoadedEventArgs>>();
builder.Services.AddSingleton<INamedEntityFormRepository, NamedEntityFormEfCoreRepository>();
builder.Services.AddServicesConfiguration(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
