using Microsoft.Extensions.Hosting;
using NewsService.Core.Abstractions;
using NewsService.Core.Events;
using NewsService.Core.Models;

namespace NewsAnalyzer.Core.Services
{
    public class BackgroundRssNewsService : BackgroundService
    {
        private readonly INewsLoader _sourceNewsLoader;
        private readonly INewsAsyncRepository _newsRepository;
        private readonly BackgroundRssNewsServiceConfiguration _configuration;


        public event EventHandler<NewsLoadedEventArgs>? NewsLoaded;

        public BackgroundRssNewsService(INewsLoader sourceNewsLoader, INewsAsyncRepository newsRepository, BackgroundRssNewsServiceConfiguration configuration)
        {
            _sourceNewsLoader = sourceNewsLoader;
            _newsRepository = newsRepository;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ExecuteLoadNewsAsync();
                await Task.Delay(_configuration.ScaningIntervalTime, stoppingToken);
            }
        }

        private async Task ExecuteLoadNewsAsync()
        {
            var newsInfos = _sourceNewsLoader.GetNewsInfos();
            var exsistedNews = await _newsRepository.GetWhereAsync(news => newsInfos
                                                                         .Select(newsInfo => newsInfo.SourceName)
                                                                         .Contains(news.SourceName));
            var newNewsInfos = newsInfos.Where(newsInfo => !exsistedNews
                                                          .Select(news => news.SourceName)
                                                          .Contains(newsInfo.SourceName));

            await foreach (var news in _sourceNewsLoader.LoadNewsAsync(newNewsInfos))
            {
                await _newsRepository.AddAsync(news);
                NewsLoaded?.Invoke(this, new NewsLoadedEventArgs { NewsId = news.Id });
            }
        }
    }
}
