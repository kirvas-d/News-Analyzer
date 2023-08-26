using Microsoft.EntityFrameworkCore;
using NlpService.Core.Models;
using NlpService.Data.Abstractions;

namespace NlpService.Data;

public class NamedEntityFormRepository : INamedEntityFormRepository
{
    private readonly NamedEntityDbContext _context;

    public NamedEntityFormRepository(NamedEntityDbContext namedEntityDbContext) 
    {
        _context = namedEntityDbContext;
    }

    public void Add(NamedEntityForm namedEntityForm)
    {
        _context.NamedEntityForms.Add(namedEntityForm);
    }

    public NamedEntityForm? GetByValue(string value)
    {
        return _context.NamedEntityForms
            .Include(e => e.NamedEntity)
            .Include(e => e.News)
            .FirstOrDefault(e => e.Value == value);
    }

    public void Remove(NamedEntityForm namedEntityForm)
    {
        _context.NamedEntityForms.Remove(namedEntityForm);
    }

    public void Update(NamedEntityForm namedEntityForm)
    {
        _context.NamedEntityForms.Update(namedEntityForm);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
