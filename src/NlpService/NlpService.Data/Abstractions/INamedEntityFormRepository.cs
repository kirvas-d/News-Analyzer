using NlpService.Core.Models;

namespace NlpService.Data.Abstractions;

public interface INamedEntityFormRepository
{
    void Add(NamedEntityForm namedEntityForm);

    NamedEntityForm? GetByValue(string value);

    void Update(NamedEntityForm namedEntityForm);

    void Remove(NamedEntityForm namedEntityForm);

    void SaveChanges();
}
