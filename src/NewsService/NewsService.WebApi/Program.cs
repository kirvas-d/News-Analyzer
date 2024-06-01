using NewsAnalyzer.Application.NewsService.Services;
using NewsAnalyzer.Core.Services;
using NewsService.Core.HtmlLoader.Abstracts;
using NewsService.Core.HtmlLoader.Services;
using NewsService.Core.NewsLoader.Abstracts;
using NewsService.Repository.NewsRepository;
using NewsService.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServicesConfiguration(builder.Configuration);
builder.Services.AddSingleton<BackgroundNewsLoaderService>();
builder.Services.AddHostedService<BackgroundRssNewsDecoratorService>();
builder.Services.AddSingleton<IHtmlLoader, SeleniumHtmlLoader>();
builder.Services.AddSingleton<INewsAsyncRepository, NewsEfCoreAsyncRepository>();

var app = builder.Build();

app.Run();
