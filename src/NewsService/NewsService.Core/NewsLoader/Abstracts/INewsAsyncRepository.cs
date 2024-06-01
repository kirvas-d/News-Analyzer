namespace NewsService.Core.NewsLoader.Abstracts;

using System.Linq.Expressions;
using NewsService.Core.NewsLoader.Models;

public interface INewsAsyncRepository
{
    Task AddAsync(News news);

    Task<News?> GetByIdAsync(Guid id);

    Task<IEnumerable<News>> GetWhereAsync(Expression<Func<News, bool>> predicate);

    Task SaveChangesAsync();
}
