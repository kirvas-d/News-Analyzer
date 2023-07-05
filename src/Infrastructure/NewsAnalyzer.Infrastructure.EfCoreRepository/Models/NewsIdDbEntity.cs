namespace NewsAnalyzer.Infrastructure.EfCoreRepository.Models;

public class NewsIdDbEntity
{
    public Guid NewsId { get; init; }

    public ICollection<NamedEntityFormDbEntity> NamedEntityFormDbEntities { get; init; }
}
