using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Core.Models;

namespace NewsAnalyzer.Infrastructure.EfCoreRepository;

public class NewsEfCoreAsyncRepository : EfCoreAsyncRepository<News>
{
    public NewsEfCoreAsyncRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
