using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.EfCoreRepository.Services;
using NewsService.Core.Abstractions;
using NewsService.Core.Models;
using System.Linq.Expressions;

namespace NewsService.Repository.NewsRepository;

public class NewsEfCoreAsyncRepository : INewsAsyncRepository
{
    private readonly NewsDbContext _context;
    public NewsEfCoreAsyncRepository(NewsDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task AddAsync(News news)
    {
        await  _context.News.AddAsync(news);
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
