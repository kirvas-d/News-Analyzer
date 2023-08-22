using NewsAnalyzer.Repository.Abstractions;
using NlpService.Core.Models;

namespace NlpService.Core.Abstractions;

public interface INamedEntityFormRepository : IGenericRepository<NamedEntityForm, Guid>
{
}
