using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Core.Models;

namespace NewsAnalyzer.Infrastructure.EfCoreRepository;

public class NamedEntityDbContext : DbContext
{
    DbSet<NamedEntity> NamedEntities { get; set; }

    DbSet<NamedEntityForm> NamedEntiryForms { get; set; }

    public NamedEntityDbContext(DbContextOptions<NamedEntityDbContext> options) : base(options) 
    {
        Database.EnsureCreated();
    }
}
