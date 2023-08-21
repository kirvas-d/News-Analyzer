using Catalyst.Models;
using Catalyst;
using Mosaik.Core;
using Version = Mosaik.Core.Version;
using NlpService.Core.Models;
using NewsService.Core.Models;
using NlpService.Core.Abstractions;
using NlpService.NerService.Helpers;

namespace NlpService.NerService.Services;

public class CatalystNerService : INerService
{
    private readonly Pipeline _pipeline;

    public CatalystNerService()
    {
        Russian.Register();

        _pipeline = Pipeline.For(Language.Russian);
        _pipeline.Add(AveragePerceptronEntityRecognizer.FromStoreAsync(Language.Russian, Version.Latest, "WikiNER").ConfigureAwait(false).GetAwaiter().GetResult());
    }

    public IEnumerable<NamedEntityForm> GetNamedEntityFormsFromNews(News news)
    {
        var doc = new Document(news.Text, Language.Russian);
        _pipeline.ProcessSingle(doc);
        doc.TokenizedValue(true);

        var namedEntityFormValueComparer = new NamedEntityFormValueComparer();
        var hashSet = new HashSet<NamedEntityForm>(doc
            .SelectMany(span => span.GetEntities())
            .Select(entity => new NamedEntityForm(entity.Value, new List<Guid> { news.Id })), namedEntityFormValueComparer);

        return hashSet;
    }
}
