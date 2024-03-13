using MassTransit;
using NewsAnalyzer.Core.Services;
using NewsService.Core.Events;
using RabbitMqService.Abstractions;

namespace NewsAnalyzer.Application.NewsService.Services;

public class BackgroundRssNewsDecoratorService : BackgroundService
{
    private readonly BackgroundRssNewsService _service;
    private readonly IMessengerPublishService<NewsLoadedEventArgs> _messengerPublishService;
    private readonly IBus _bus;

    public BackgroundRssNewsDecoratorService(BackgroundRssNewsService service, IMessengerPublishService<NewsLoadedEventArgs> messengerPublishService, IBus bus)
    {
        _service = service;
        _messengerPublishService = messengerPublishService;
        _bus = bus;

        _service.NewsLoaded += NewsLoaded;
    }

    private async void NewsLoaded(object? sender, NewsLoadedEventArgs e)
    {
        //_messengerPublishService.PublishMessage(e);
        await _bus.Publish(e);
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