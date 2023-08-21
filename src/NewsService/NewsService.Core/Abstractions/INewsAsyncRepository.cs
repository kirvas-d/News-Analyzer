using NewsAnalyzer.Core.Abstractions;
using NewsService.Core.Models;

namespace NewsService.Core.Abstractions;

public interface INewsAsyncRepository : IAsyncGenericRepository<News, Guid>
{
}
