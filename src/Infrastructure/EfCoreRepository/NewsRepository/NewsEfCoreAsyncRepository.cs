using NewsService.Core.Abstractions;
using NewsService.Core.Models;

namespace EfCoreRepository.NewsRepository;

public class NewsEfCoreAsyncRepository : EfCoreAsyncRepository<News, Guid>, INewsAsyncRepository
{
    public NewsEfCoreAsyncRepository(NewsDbContext dbContext) : base(dbContext)
    {
    }
}
