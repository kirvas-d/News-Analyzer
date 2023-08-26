using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NlpService.Core.Models;
using System.Text.Json;

namespace NlpService.Data;

public class NamedEntityDbContext : DbContext
{
    public DbSet<NamedEntity> NamedEntities { get; set; }
    public DbSet<NamedEntityForm> NamedEntityForms { get; set; }
    public DbSet<News> News { get; set; }

    public NamedEntityDbContext(DbContextOptions<NamedEntityDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.General);

        modelBuilder.Entity<News>()
            .Property(e => e.SentimentAnalyzeResult)
            .HasConversion(new ValueConverter<SentimentAnalyzeResult, string>(
                news => JsonSerializer.Serialize(news, options),
                news => JsonSerializer.Deserialize<SentimentAnalyzeResult>(news, options)));

        modelBuilder.Entity<NamedEntityForm>()
            .HasKey(e => e.Value);
    }


}