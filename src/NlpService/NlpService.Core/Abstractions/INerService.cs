using NewsService.Core.Models;
using NlpService.Core.Models;

namespace NlpService.Core.Abstractions;

public interface INerService
{
    IEnumerable<NamedEntityForm> GetNamedEntityFormsFromNews(News news);
}
