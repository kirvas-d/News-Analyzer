using Microsoft.EntityFrameworkCore;
using NewsService.Core.NewsLoader.Models;

namespace NewsService.Repository.NewsRepository;

public class NewsDbContext : DbContext
{
    public DbSet<News> News { get; set; }

    public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
