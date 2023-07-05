using NewsAnalyzer.Core.Abstractions;
using NewsAnalyzer.Core.Models;
using NewsAnalyzer.Infrastructure.EfCoreRepository.NewsRepository;

namespace NewsAnalyzer.Infrastructure.EfCoreRepository.NamedEntityFormRepository;

public class NamedEntityFormEfCoreRepository : EfCoreRepository<NamedEntityForm, Guid>, INamedEntityFormRepository
{
    public NamedEntityFormEfCoreRepository(NamedEntityFormDbContext dbContext) : base(dbContext)
    {
    }
}
