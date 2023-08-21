namespace EfCoreRepository.Models;

public class NamedEntityFormDbEntity
{
    public Guid Id { get; init; }

    public NamedEntityDbEntity? NamedEntity { get; init; }

    public string Value { get; init; }

    public ICollection<NewsIdDbEntity> NewsIds { get; init; }
}
