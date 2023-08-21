namespace NewsService.Core.Models;

public class News
{
    public Guid Id { get; init; }
    public string SourceName { get; init; }
    public string? Title { get; init; }
    public string Text { get; init; }
    public DateTime PublishDate { get; init; }

    public News(Guid id, string sourceName, string? title, string text, DateTime publishDate)
    {
        Id = id;
        SourceName = sourceName;
        Title = title;
        Text = text;
        PublishDate = publishDate;
    }
}