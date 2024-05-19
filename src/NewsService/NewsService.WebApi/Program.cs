using NewsAnalyzer.Application.NewsService.Services;
using NewsAnalyzer.Core.Services;
using NewsService.Core.HtmlLoader.Abstracts;
using NewsService.Core.HtmlLoader.Services;
using NewsService.Core.NewsLoader.Abstracts;
using NewsService.Repository.NewsRepository;
using NewsService.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddServicesConfiguration(builder.Configuration);
builder.Services.AddSingleton<BackgroundNewsLoaderService>();
builder.Services.AddHostedService<BackgroundRssNewsDecoratorService>();
builder.Services.AddSingleton<IHtmlLoader, SeleniumHtmlLoader>();
builder.Services.AddSingleton<INewsAsyncRepository, NewsEfCoreAsyncRepository>();

var app = builder.Build();

app.MapGrpcService<ApplicationNewsService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
