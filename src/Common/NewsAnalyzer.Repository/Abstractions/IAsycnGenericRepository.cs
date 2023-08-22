using System.Linq.Expressions;

namespace NewsAnalyzer.Repository.Abstractions;

public interface IAsyncGenericRepository<TEntity, TId> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(TId id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

    Task AddAsync(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities);

    Task RemoveAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task<int> CountAllAsync();
}
