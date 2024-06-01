namespace NewsService.Repository.NewsRepository;

using Microsoft.EntityFrameworkCore;
using NewsService.Core.NewsLoader.Models;

public class NewsDbContext : DbContext
{
    public DbSet<News> News { get; set; }

    public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
