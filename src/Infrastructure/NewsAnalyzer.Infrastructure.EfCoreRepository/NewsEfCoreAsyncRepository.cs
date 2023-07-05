using NewsAnalyzer.Core.Abstractions;
using NewsAnalyzer.Core.Models;

namespace NewsAnalyzer.Infrastructure.EfCoreRepository;

public class NewsEfCoreAsyncRepository : EfCoreAsyncRepository<News, Guid>, INewsAsyncRepository
{
    public NewsEfCoreAsyncRepository(NewsDbContext dbContext) : base(dbContext)
    {
    }
}
