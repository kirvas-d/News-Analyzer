namespace NlpService.Core.Models;

public class NamedEntityForm
{
    private readonly HashSet<Text> _texts;

    public NamedEntity? NamedEntity { get; init; }

    public string Value { get; init; }

    public IReadOnlyCollection<Text> Texts => _texts;

    private NamedEntityForm() { }

    public NamedEntityForm(string value, IReadOnlyCollection<Text> news)
    {
        Value = value;
        _texts = new HashSet<Text>(news);
    }

    public void AddNews(Text news)
    {
        _texts.Add(news);
    }
}