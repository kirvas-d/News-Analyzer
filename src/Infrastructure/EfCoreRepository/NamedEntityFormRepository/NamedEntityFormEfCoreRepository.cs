using NewsAnalyzer.Core.Abstractions;
using NlpService.Core.Models;

namespace EfCoreRepository.NamedEntityFormRepository;

public class NamedEntityFormEfCoreRepository : EfCoreRepository<NamedEntityForm, Guid>, INamedEntityFormRepository
{
    public NamedEntityFormEfCoreRepository(NamedEntityFormDbContext dbContext) : base(dbContext)
    {
    }
}
