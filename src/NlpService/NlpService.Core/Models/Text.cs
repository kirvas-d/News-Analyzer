namespace NlpService.Core.Models;

public class Text
{
    private readonly List<NamedEntityForm> _namedEntityForms;

    public Guid Id { get; init; }

    public IReadOnlyList<NamedEntityForm> NamedEntityForms => _namedEntityForms;

    private Text()
    {
        Id = Guid.NewGuid();
        _namedEntityForms = new List<NamedEntityForm>();
    }

    public Text(Guid id, SentimentAnalyzeResult sentimentAnalyzeResult, IEnumerable<NamedEntityForm>? namedEntityForms = null)
    {
        Id = id;
        _namedEntityForms = new List<NamedEntityForm>();
        if (namedEntityForms != null)
            _namedEntityForms.AddRange(namedEntityForms);
    }
}
