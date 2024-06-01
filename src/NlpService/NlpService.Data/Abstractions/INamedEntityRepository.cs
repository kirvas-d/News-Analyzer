namespace NlpService.Data.Abstractions;

using NlpService.Core.Models;

public interface INamedEntityRepository
{
    void Add(NamedEntity namedEntity);

    NamedEntity? GetById(Guid id);

    IEnumerable<NamedEntity> GetAll();

    void Update(NamedEntity namedEntity);

    void Remove(NamedEntity namedEntity);

    void SaveChanges();
}
