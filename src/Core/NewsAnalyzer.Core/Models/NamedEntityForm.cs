namespace NewsAnalyzer.Core.Models;

public class NamedEntityForm
{
    private readonly HashSet<Guid> _newsIds;

    public Guid Id { get; init; }

    public NamedEntity? NamedEntity { get; init; }

    public string Value { get; init; }

    public IReadOnlyCollection<Guid> NewsIds => _newsIds;

    public NamedEntityForm(Guid id, string value,  IEnumerable<Guid> newsIds) 
    {
        Id = id;
        Value = value;
        _newsIds = new HashSet<Guid>(newsIds);
    }

    public NamedEntityForm(string value, IReadOnlyCollection<Guid> newsIds) : this(Guid.NewGuid(), value, newsIds)
    { }

    public void AddNewsId(Guid newsId) 
    {
        _newsIds.Add(newsId);
    }
}