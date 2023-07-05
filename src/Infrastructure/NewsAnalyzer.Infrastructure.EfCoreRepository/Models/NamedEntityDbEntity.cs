using NewsAnalyzer.Core.Models;

namespace NewsAnalyzer.Infrastructure.EfCoreRepository.Models;

public class NamedEntityDbEntity
{
    public Guid Id { get; init; }

    public string Value { get; init; }

    public IReadOnlyList<NamedEntityFormDbEntity>? NamedEntityForms { get; init; }
}
