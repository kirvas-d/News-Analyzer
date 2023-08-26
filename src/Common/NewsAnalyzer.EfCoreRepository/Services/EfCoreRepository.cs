using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace NewsAnalyzer.EfCoreRepository.Services;

public class EfCoreRepository<TDbContext, TEntity, TId> : IDisposable
    where TDbContext : DbContext
    where TEntity : class 
{
    protected readonly DbSet<TEntity> _dbSet;
    protected TDbContext _context;

    public EfCoreRepository(TDbContext dbContext)
    {
        _context = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    public virtual void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);      
    }

    public virtual void AddRange(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    public virtual TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.FirstOrDefault(predicate);
    }

    public virtual IEnumerable<TEntity> GetAll()
    {
        return _dbSet.ToList();
    }

    public virtual TEntity? GetById(TId id)
    {
        return _dbSet.Find(id);
    }

    public virtual IEnumerable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }

    public virtual void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void SaveChanges()
    {
        _context.SaveChanges();
    }

    public virtual void Dispose()
    {
        _context.Dispose();
    }
}
