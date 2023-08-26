using Microsoft.EntityFrameworkCore;
using NlpService.Core.Models;
using NlpService.Data.Abstractions;

namespace NlpService.Data;

public class NewsRepository : INewsRepository
{
    private readonly NamedEntityDbContext _context;

    public NewsRepository(NamedEntityDbContext namedEntityDbContext) 
    {
        _context = namedEntityDbContext;
    }

    public void Add(News news)
    {
        _context.News.Add(news);
    }

    public News? GetById(Guid id)
    {
        return _context.News
            .Include(e => e.NamedEntityForms)
            .FirstOrDefault(e => e.Id == id);
    }

    public void Update(News news)
    {
        _context.News.Update(news);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
