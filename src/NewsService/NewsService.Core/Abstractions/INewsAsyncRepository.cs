using NewsAnalyzer.Repository.Abstractions;
using NewsService.Core.Models;

namespace NewsService.Core.Abstractions;

public interface INewsAsyncRepository : IAsyncGenericRepository<News, Guid>
{
}
