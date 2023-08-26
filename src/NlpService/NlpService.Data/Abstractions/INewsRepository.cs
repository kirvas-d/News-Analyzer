using NlpService.Core.Models;

namespace NlpService.Data.Abstractions;

public interface INewsRepository
{
    void Add(News news);

    void Update(News news);

    News? GetById(Guid id);

    void SaveChanges();
}
