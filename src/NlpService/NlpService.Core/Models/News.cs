namespace NlpService.Core.Models;

public class News
{
    private readonly List<NamedEntityForm> _namedEntityForms;

    public Guid Id { get; init; }

    public SentimentAnalyzeResult SentimentAnalyzeResult { get; init;}

    public IReadOnlyList<NamedEntityForm> NamedEntityForms => _namedEntityForms;

    private News() { }

    public News(Guid id, SentimentAnalyzeResult sentimentAnalyzeResult, IEnumerable<NamedEntityForm>? namedEntityForms = null) 
    {
        Id = id;
        SentimentAnalyzeResult = sentimentAnalyzeResult;
        _namedEntityForms = new List<NamedEntityForm>();
        if (namedEntityForms != null)
            _namedEntityForms.AddRange(namedEntityForms);
    }
}
