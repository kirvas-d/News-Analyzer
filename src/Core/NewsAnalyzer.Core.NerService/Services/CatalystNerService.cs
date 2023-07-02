using Catalyst.Models;
using Catalyst;
using NewsAnalyzer.Core.Abstractions;
using NewsAnalyzer.Core.Models;
using Mosaik.Core;
using Version = Mosaik.Core.Version;
using NewsAnalyzer.Core.NerService.Helpers;

namespace NewsAnalyzer.Core.NerService.Services;

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
            .Select(entity => new NamedEntityForm(entity.Value, news.Id)), namedEntityFormValueComparer);

        return hashSet;
    }
}
