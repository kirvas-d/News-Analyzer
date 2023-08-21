using NewsAnalyzer.Core.Services;
using NewsService.Core.Events;
using RabbitMqService.Abstractions;

namespace NewsAnalyzer.Application.NewsService.Services;

public class BackgroundRssNewsDecoratorService : BackgroundService
{
    private readonly BackgroundRssNewsService _service;
    private readonly IMessengerPublishService<NewsLoadedEventArgs> _messengerPublishService;

    public BackgroundRssNewsDecoratorService(BackgroundRssNewsService service, IMessengerPublishService<NewsLoadedEventArgs> messengerPublishService)
    {
        _service = service;
        _messengerPublishService = messengerPublishService;

        _service.NewsLoaded += NewsLoaded;
    }

    private void NewsLoaded(object? sender, NewsLoadedEventArgs e)
    {
        _messengerPublishService.PublishMessage(e);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {        
            await _service.StartAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromDays(1));
        }
    }
}