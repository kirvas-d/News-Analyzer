using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Core.Abstractions;
using NewsAnalyzer.Core.Models;

namespace NewsAnalyzer.Infrastructure.EfCoreRepository;

public class NewsEfCoreAsyncRepository : EfCoreAsyncRepository<News>, INewsAsyncRepository
{
    public NewsEfCoreAsyncRepository(NewsDbContext dbContext) : base(dbContext)
    {
    }
}
