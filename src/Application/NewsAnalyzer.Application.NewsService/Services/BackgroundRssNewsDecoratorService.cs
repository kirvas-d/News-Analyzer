using NewsAnalyzer.Core.Abstractions;
using NewsAnalyzer.Core.Events;
using NewsAnalyzer.Core.Services;
using NewsAnalyzer.Infrastructure.RabbitMqService.Abstractions;

namespace NewsAnalyzer.Application.NewsService.Services
{
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
                try
                {
                    await _service.StartAsync(stoppingToken);
                    //await ExecuteLoadNewsAsync();
                }
                catch (Exception exception)
                {
                    //_logger.LogError(exception, $"Faild {nameof(BackgroundRssNewsDecoratorService)}");
                }

                await Task.Delay(TimeSpan.FromDays(1));
            }
        }

        //private async Task ExecuteLoadNewsAsync() 
        //{
        //    await foreach (var news in _sourceNewsLoader.LoadNewsAsync()) 
        //    {
        //        var exsistedNews = await _newsRepository.FirstOrDefaultAsync(n => n.SourceName == news.SourceName);
        //        if(exsistedNews == null)
        //            await _newsRepository.AddAsync(news);
        //    }
        //} 
    }
}
