using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NewsAnalyzer.Core.Models;
using System.Text.Json;

namespace NewsAnalyzer.Infrastructure.EfCoreRepository.NamedEntityFormRepository;

public class NamedEntityFormDbContext : DbContext
{
    public DbSet<NamedEntityForm> namedEntityForms { get; set; }

    public NamedEntityFormDbContext(DbContextOptions<NamedEntityFormDbContext> options) : base(options) 
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.General);

        modelBuilder.Entity<NamedEntityForm>()
            .Property(e => e.NewsIds)
            .HasConversion(new ValueConverter<IReadOnlyCollection<Guid>, string>(
                news => JsonSerializer.Serialize(news, options),
                news => JsonSerializer.Deserialize<IReadOnlyCollection<Guid>>(news, options)));
    }

    
}