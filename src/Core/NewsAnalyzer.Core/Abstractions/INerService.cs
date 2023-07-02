using NewsAnalyzer.Core.Models;

namespace NewsAnalyzer.Core.Abstractions;

public interface INerService
{
    IEnumerable<NamedEntityForm> GetNamedEntityFormsFromNews(News news);
}
