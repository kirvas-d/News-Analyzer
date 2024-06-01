namespace NewsService.Core.NewsLoader.Abstracts;

using NewsService.Core.NewsLoader.Models;

public interface INewsLoader
{
    IEnumerable<NewsInfo> GetNewsInfos();

    IAsyncEnumerable<News> LoadNewsAsync(IEnumerable<NewsInfo> newsInfos);
}
