namespace NewsService.Repository.NewsRepository;

using Microsoft.EntityFrameworkCore;
using NewsService.Core.NewsLoader.Abstracts;
using NewsService.Core.NewsLoader.Models;
using System.Linq.Expressions;

public class NewsEfCoreAsyncRepository : INewsAsyncRepository
{
    private readonly NewsDbContext _context;
    public NewsEfCoreAsyncRepository(NewsDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task AddAsync(News news)
    {
        await _context.News.AddAsync(news);
    }

    public async Task<News?> GetByIdAsync(Guid id)
    {
        return await _context.News.FindAsync(id);
    }

    public async Task<IEnumerable<News>> GetWhereAsync(Expression<Func<News, bool>> predicate)
    {
        return await _context.News.Where(predicate).ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
