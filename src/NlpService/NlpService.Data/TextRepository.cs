using Microsoft.EntityFrameworkCore;
using NlpService.Core.Models;
using NlpService.Data.Abstractions;

namespace NlpService.Data;

public class TextRepository : ITextRepository
{
    private readonly NamedEntityDbContext _context;

    public TextRepository(NamedEntityDbContext namedEntityDbContext) 
    {
        _context = namedEntityDbContext;
    }

    public void Add(Text news)
    {
        _context.Texts.Add(news);
    }

    public Text? GetById(Guid id)
    {
        return _context.Texts
            .Include(e => e.NamedEntityForms)
            .FirstOrDefault(e => e.Id == id);
    }

    public void Update(Text news)
    {
        _context.Texts.Update(news);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
