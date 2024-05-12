using MassTransit;
using NewsAnalyzer.Core.Services;
using NewsService.Core.NewsLoader.Events;

namespace NewsAnalyzer.Application.NewsService.Services;

public class BackgroundRssNewsDecoratorService : BackgroundService
{
    private readonly BackgroundNewsLoaderService _service;
    private readonly IBus _bus;

    public BackgroundRssNewsDecoratorService(BackgroundNewsLoaderService service, IBus bus)
    {
        _service = service;
        _bus = bus;

        _service.NewsLoaded += NewsLoaded;
    }

    private async void NewsLoaded(object? sender, NewsLoadedEventArgs e)
    {
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