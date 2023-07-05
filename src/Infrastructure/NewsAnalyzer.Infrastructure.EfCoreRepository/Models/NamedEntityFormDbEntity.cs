using NewsAnalyzer.Core.Models;

namespace NewsAnalyzer.Infrastructure.EfCoreRepository.Models;

public class NamedEntityFormDbEntity
{
    public Guid Id { get; init; }

    public NamedEntityDbEntity? NamedEntity { get; init; }

    public string Value { get; init; }

    public ICollection<NewsIdDbEntity> NewsIds { get; init; }
}
