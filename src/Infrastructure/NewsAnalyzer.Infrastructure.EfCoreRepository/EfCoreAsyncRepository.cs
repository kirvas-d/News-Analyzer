
using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Core.Abstractions;
using System.Linq.Expressions;

namespace NewsAnalyzer.Infrastructure.EfCoreRepository;

public class EfCoreAsyncRepository<TEntity, TId> : IAsyncGenericRepository<TEntity, TId> where TEntity : class
{

    protected DbContext _context;

    protected EfCoreAsyncRepository(DbContext dbContext) 
    {
        _context = dbContext;
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _context.Set<TEntity>().AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<int> CountAllAsync()
    {
        return await _context.Set<TEntity>().CountAsync();
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id) 
    {
        return await _context.Set<TEntity>().FindAsync(id).AsTask();
    }

    public virtual async Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public virtual Task RemoveAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        return _context.SaveChangesAsync();
    }

    public virtual Task UpdateAsync(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return _context.SaveChangesAsync();
    }
}
