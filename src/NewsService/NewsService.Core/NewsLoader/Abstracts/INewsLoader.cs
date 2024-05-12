using NewsService.Core.NewsLoader.Models;

namespace NewsService.Core.NewsLoader.Abstracts;

public interface INewsLoader
{
    IEnumerable<NewsInfo> GetNewsInfos();

    IAsyncEnumerable<News> LoadNewsAsync(IEnumerable<NewsInfo> newsInfos);
}
