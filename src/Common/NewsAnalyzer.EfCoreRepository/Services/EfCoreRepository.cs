using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Repository.Abstractions;
using System.Linq.Expressions;

namespace NewsAnalyzer.EfCoreRepository.Services;

public class EfCoreRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : class
{
    protected DbContext _context;

    public EfCoreRepository(DbContext dbContext)
    {
        _context = dbContext;
    }

    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        _context.SaveChanges();
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().AddRange(entities);
        _context.SaveChanges();
    }

    public int CountAll()
    {
        return _context.Set<TEntity>().Count();
    }

    public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return _context.Set<TEntity>().FirstOrDefault(predicate);
    }

    public IEnumerable<TEntity> GetAll()
    {
        return _context.Set<TEntity>().ToList();
    }

    public TEntity? GetById(TId id)
    {
        return _context.Set<TEntity>().Find(id);
    }

    public IEnumerable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate)
    {
        return _context.Set<TEntity>().Where(predicate);
    }

    public void Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        _context.SaveChanges();
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        _context.SaveChanges();
    }
}
