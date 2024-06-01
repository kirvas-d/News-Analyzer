namespace NlpService.Data.Abstractions;

using NlpService.Core.Models;

public interface INamedEntityFormRepository
{
    void Add(NamedEntityForm namedEntityForm);

    NamedEntityForm? GetByValue(string value);

    void Update(NamedEntityForm namedEntityForm);

    void Remove(NamedEntityForm namedEntityForm);

    void SaveChanges();
}
