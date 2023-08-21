using NewsService.Core.Models;

namespace NewsService.Core.Abstractions;

public interface INewsLoader
{
    IEnumerable<NewsInfo> GetNewsInfos();

    IAsyncEnumerable<News> LoadNewsAsync(IEnumerable<NewsInfo> newsInfos);
}
