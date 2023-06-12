using System.Linq.Expressions;

namespace NewsAnalyzer.Core.Abstractions;

public interface IGenericRepository<T> where T : class
{
    T GetById(int id);

    IEnumerable<T> GetAll();

    IEnumerable<T> GetWhereA(Expression<Func<T, bool>> predicate);

    T FirstOrDefault(Expression<Func<T, bool>> predicate);

    void Add(T entity);

    void AddRange(IEnumerable<T> entities);

    void Remove(T entity);

    void Update(T entity);

    int CountAll();
}
