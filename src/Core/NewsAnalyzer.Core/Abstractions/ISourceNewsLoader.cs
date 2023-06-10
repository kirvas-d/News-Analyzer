using NewsAnalyzer.Core.Models;

namespace NewsAnalyzer.Core.Abstractions;

public interface ISourceNewsLoader
{
    IAsyncEnumerable<News> LoadNewsAsync();
}
