
using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Core.Abstractions;
using System.Linq.Expressions;

namespace NewsAnalyzer.Infrastructure.EfCoreRepository;

public class EfCoreAsyncRepository<T> : IAsyncGenericRepository<T> where T : class
{

    protected DbContext _context;

    protected EfCoreAsyncRepository(DbContext dbContext) 
    {
        _context = dbContext;
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountAllAsync()
    {
        return await _context.Set<T>().CountAsync();
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id) 
    {
        return await _context.Set<T>().FindAsync(id).AsTask();
    }

    public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public Task RemoveAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        return _context.SaveChangesAsync();
    }

    public Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return _context.SaveChangesAsync();
    }
}
