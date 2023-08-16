using NewsAnalyzer.Core.Models;

namespace NewsAnalyzer.Core.Abstractions
{
    public interface INamedEntityFormAsyncRepository : IAsyncGenericRepository<NamedEntityForm, Guid>
    {
        Task<IEnumerable<NamedEntityForm>?> GetByValueAsync(IEnumerable<string> values);
    }
}
