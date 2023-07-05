using System.Linq.Expressions;

namespace NewsAnalyzer.Core.Abstractions;

public interface IGenericRepository<TEntity, TId> where TEntity : class
{
    TEntity GetById(TId id);

    IEnumerable<TEntity> GetAll();

    IEnumerable<TEntity> GetWhereA(Expression<Func<TEntity, bool>> predicate);

    TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void Update(TEntity entity);

    int CountAll();
}
