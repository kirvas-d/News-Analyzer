using NewsAnalyzer.EfCoreRepository.Services;
using NlpService.Core.Abstractions;
using NlpService.Core.Models;

namespace NlpService.Repository.NamedEntityFormRepository;

public class NamedEntityFormEfCoreRepository : EfCoreRepository<NamedEntityForm, Guid>, INamedEntityFormRepository
{
    public NamedEntityFormEfCoreRepository(NamedEntityFormDbContext dbContext) : base(dbContext)
    {
    }
}
