using Microsoft.EntityFrameworkCore;
using NewsService.Core.Models;

namespace NewsService.Repository.NewsRepository;

public class NewsDbContext : DbContext
{
    DbSet<News> News { get; set; }

    public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
