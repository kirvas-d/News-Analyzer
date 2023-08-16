using NewsAnalyzer.Core.Models;

namespace NewsAnalyzer.Core.Abstractions;

public interface INewsLoader
{
    IEnumerable<NewsInfo> GetNewsInfos();

    IAsyncEnumerable<News> LoadNewsAsync(IEnumerable<NewsInfo> newsInfos);
}
