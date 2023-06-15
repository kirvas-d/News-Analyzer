using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Core.Models;

namespace NewsAnalyzer.Infrastructure.EfCoreRepository;

public class NewsDbContext : DbContext
{
    DbSet<News> News { get; set; }

    public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
