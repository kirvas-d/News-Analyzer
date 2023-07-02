namespace NewsAnalyzer.Core.Models;

public class NamedEntityForm
{
    private readonly HashSet<Guid> _newsIds;

    public Guid Id { get; init; }

    public NamedEntity? NamedEntity { get; init; }

    public string Value { get; init; }

    public IReadOnlyCollection<Guid> NewsIds => _newsIds;

    public NamedEntityForm(Guid id, string value, Guid newsId) 
    {
        Id = id;
        Value = value;
        _newsIds = new HashSet<Guid>
        {
            newsId
        };
    }

    public NamedEntityForm(string value, Guid newsId) : this(Guid.NewGuid(), value, newsId)
    { }

    public void AddNewsId(Guid newsId) 
    {
        _newsIds.Add(newsId);
    }
}