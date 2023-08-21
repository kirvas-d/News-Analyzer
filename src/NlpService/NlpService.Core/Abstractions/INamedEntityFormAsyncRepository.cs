using NewsAnalyzer.Core.Abstractions;
using NlpService.Core.Models;

namespace NlpService.Core.Abstractions;

public interface INamedEntityFormAsyncRepository : IAsyncGenericRepository<NamedEntityForm, Guid>
{
    Task<IEnumerable<NamedEntityForm>?> GetByValueAsync(IEnumerable<string> values);
}
