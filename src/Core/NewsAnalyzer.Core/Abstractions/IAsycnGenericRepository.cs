using System.Linq.Expressions;

namespace NewsAnalyzer.Core.Abstractions;

public interface IAsyncRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);

    Task<IEnumerable<T>> GetAllAsync();

    Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);

    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    Task AddAsync(T entity);

    Task AddRangeAsync(IEnumerable<T> entities);

    Task RemoveAsync(T entity);

    Task UpdateAsync(T entity);

    Task<int> CountAllAsync();
}
