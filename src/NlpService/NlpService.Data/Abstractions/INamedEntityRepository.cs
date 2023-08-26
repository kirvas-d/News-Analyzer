using NlpService.Core.Models;

namespace NlpService.Data.Abstractions;

public interface INamedEntityRepository
{
    void Add(NamedEntity namedEntity);

    NamedEntity? GetById(Guid id);

    IEnumerable<NamedEntity> GetAll();

    void Update(NamedEntity namedEntity);

    void Remove(NamedEntity namedEntity);

    void SaveChanges();
}
