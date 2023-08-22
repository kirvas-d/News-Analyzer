using NewsAnalyzer.EfCoreRepository.Services;
using NewsService.Core.Abstractions;
using NewsService.Core.Models;

namespace NewsService.Repository.NewsRepository;

public class NewsEfCoreAsyncRepository : EfCoreAsyncRepository<News, Guid>, INewsAsyncRepository
{
    public NewsEfCoreAsyncRepository(NewsDbContext dbContext) : base(dbContext)
    {
    }
}
