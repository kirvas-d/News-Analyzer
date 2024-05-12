using NlpService.Core.Models;

namespace NlpService.Data.Abstractions;

public interface ITextRepository
{
    void Add(Text news);

    void Update(Text news);

    Text? GetById(Guid id);

    void SaveChanges();
}
