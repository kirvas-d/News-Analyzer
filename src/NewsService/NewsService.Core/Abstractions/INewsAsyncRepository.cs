using NewsService.Core.Models;
using System.Linq.Expressions;

namespace NewsService.Core.Abstractions;

public interface INewsAsyncRepository
{
    Task AddAsync(News news);

    Task<News?> GetByIdAsync(Guid id);

    Task<IEnumerable<News>> GetWhereAsync(Expression<Func<News, bool>> predicate);

    Task SaveChangesAsync();
}
