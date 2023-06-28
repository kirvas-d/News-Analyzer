using Microsoft.Extensions.Hosting;
using NewsAnalyzer.Core.Abstractions;
using NewsAnalyzer.Core.Events;
using NewsAnalyzer.Core.Models;

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
            await foreach (var news in _sourceNewsLoader.LoadNewsAsync())
            {
                var exsistedNews = await _newsRepository.FirstOrDefaultAsync(n => n.SourceName == news.SourceName);
                if (exsistedNews == null)
                {
                    await _newsRepository.AddAsync(news);
                    NewsLoaded?.Invoke(this, new NewsLoadedEventArgs { NewsId = news.Id });
                }
            }
        }
    }
}
