using EfCoreRepository.NewsRepository;
using NewsAnalyzer.Application.NewsService.Extensions;
using NewsAnalyzer.Application.NewsService.Services;
using NewsAnalyzer.Core.Services;
using NewsService.Core.Abstractions;
using NewsService.Core.Events;
using RabbitMqService.Abstractions;
using RabbitMqService.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddServicesConfiguration(builder.Configuration);
builder.Services.AddSingleton<BackgroundRssNewsService>();
builder.Services.AddHostedService<BackgroundRssNewsDecoratorService>();
builder.Services.AddSingleton<IHtmlLoader, PlayWrightHtmlLoader>();
builder.Services.AddSingleton<INewsLoader, RssNewsLoader>();
builder.Services.AddSingleton<INewsAsyncRepository, NewsEfCoreAsyncRepository>();
builder.Services.AddSingleton<IMessengerPublishService<NewsLoadedEventArgs>, RabbitMqMessengerPublishService<NewsLoadedEventArgs>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ApplicationNewsService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
