using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Infrastructure.EfCoreRepository.Models;

namespace NewsAnalyzer.Infrastructure.EfCoreRepository;

public class NamedEntityDbContext : DbContext
{
    public DbSet<NamedEntityDbEntity> NamedEntities { get; set; }

    public DbSet<NamedEntityFormDbEntity> NamedEntityFormDbEntities { get; set; }

    public DbSet<NewsIdDbEntity> NewsIdDbEntyties { get; set; }

    public NamedEntityDbContext(DbContextOptions<NamedEntityDbContext> options) : base(options) 
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NewsIdDbEntity>().HasKey("NewsId");

        modelBuilder.Entity<NamedEntityFormDbEntity>()
            .HasMany(e => e.NewsIds)
            .WithMany(n => n.NamedEntityFormDbEntities);
    }
}
