namespace EfCoreRepository.Models;

public class NamedEntityDbEntity
{
    public Guid Id { get; init; }

    public string Value { get; init; }

    public IReadOnlyList<NamedEntityFormDbEntity>? NamedEntityForms { get; init; }
}
