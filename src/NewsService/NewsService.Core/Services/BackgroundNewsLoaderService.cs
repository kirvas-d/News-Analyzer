namespace NewsAnalyzer.Core.Services;

using Microsoft.Extensions.Hosting;
using NewsService.Core.NewsLoader.Abstracts;
using NewsService.Core.NewsLoader.Events;
using NewsService.Core.Services;

public class BackgroundNewsLoaderService : BackgroundService
{
    private readonly IEnumerable<INewsLoader> _newsLoaders;
    private readonly INewsAsyncRepository _newsRepository;
    private readonly BackgroundNewsServiceConfiguration _configuration;

    public event EventHandler<NewsLoadedEventArgs>? NewsLoaded;

    public BackgroundNewsLoaderService(
        IEnumerable<INewsLoader> newsLoaders,
        INewsAsyncRepository newsRepository,
        BackgroundNewsServiceConfiguration configuration)
    {
        _newsLoaders = newsLoaders;
        _newsRepository = newsRepository;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await ExecuteLoadNewsAsync(cancellationToken);
            await Task.Delay(_configuration.ScaningIntervalTime, cancellationToken);
        }
    }

    private async Task ExecuteLoadNewsAsync(CancellationToken cancellationToken)
    {
        foreach (var newsLoader in _newsLoaders)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            var newsInfos = newsLoader.GetNewsInfos();
            var exsistedNews = await _newsRepository
                .GetWhereAsync(news => newsInfos
                .Select(newsInfo => newsInfo.SourceName)
                .Contains(news.SourceName));

            var newNewsInfos = newsInfos
                .Where(newsInfo => !exsistedNews
                .Select(news => news.SourceName)
                .Contains(newsInfo.SourceName));

            await foreach (var news in newsLoader.LoadNewsAsync(newNewsInfos))
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                await _newsRepository.AddAsync(news);
                await _newsRepository.SaveChangesAsync();
                NewsLoaded?.Invoke(this, new NewsLoadedEventArgs
                {
                    Id = news.Id,
                    PublishDate = news.PublishDate,
                    SourceName = news.SourceName,
                    Text = news.Text,
                    Title = news.Title,
                });
            }
        }
    }
}
