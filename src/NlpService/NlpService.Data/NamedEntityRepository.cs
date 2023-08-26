using Microsoft.EntityFrameworkCore;
using NlpService.Core.Models;
using NlpService.Data.Abstractions;

namespace NlpService.Data;

public class NamedEntityRepository : INamedEntityRepository
{
    private readonly NamedEntityDbContext _context;

    public NamedEntityRepository(NamedEntityDbContext namedEntityDbContext)
    {
        _context = namedEntityDbContext;
    }

    public void Add(NamedEntity namedEntity)
    {
        _context.NamedEntities.Add(namedEntity);
    }

    public IEnumerable<NamedEntity> GetAll()
    {
        return _context.NamedEntities
            .Include(e => e.NamedEntityForms) 
            .ToList();
    }

    public NamedEntity? GetById(Guid id)
    {
        return _context.NamedEntities
            .Include(e => e.NamedEntityForms)
            .FirstOrDefault(e => e.Id == id);
    }

    public void Remove(NamedEntity namedEntity)
    {
        _context.NamedEntities.Remove(namedEntity);
    }

    public void Update(NamedEntity namedEntity)
    {
        _context.NamedEntities.Update(namedEntity);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
