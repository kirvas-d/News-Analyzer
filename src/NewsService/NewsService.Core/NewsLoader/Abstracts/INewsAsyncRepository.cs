using System.Linq.Expressions;
using NewsService.Core.NewsLoader.Models;

namespace NewsService.Core.NewsLoader.Abstracts;

public interface INewsAsyncRepository
{
    Task AddAsync(News news);

    Task<News?> GetByIdAsync(Guid id);

    Task<IEnumerable<News>> GetWhereAsync(Expression<Func<News, bool>> predicate);

    Task SaveChangesAsync();
}
