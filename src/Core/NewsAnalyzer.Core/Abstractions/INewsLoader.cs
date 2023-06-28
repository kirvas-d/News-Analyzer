using NewsAnalyzer.Core.Models;

namespace NewsAnalyzer.Core.Abstractions;

public interface INewsLoader
{
    IAsyncEnumerable<News> LoadNewsAsync();
}
