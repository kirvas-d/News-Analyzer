namespace NlpService.Core.Models;

public class NamedEntityForm
{
    private readonly HashSet<News> _news;

    public NamedEntity? NamedEntity { get; init; }

    public string Value { get; init; }

    public IReadOnlyCollection<News> News => _news;

    private NamedEntityForm() { }

    public NamedEntityForm(string value, IReadOnlyCollection<News> news)
    {
        Value = value;
        _news = new HashSet<News>(news);
    }

    public void AddNews(News news)
    {
        _news.Add(news);
    }
}