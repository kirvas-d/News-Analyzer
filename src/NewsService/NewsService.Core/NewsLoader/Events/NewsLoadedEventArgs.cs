namespace NewsService.Core.NewsLoader.Events;

public class NewsLoadedEventArgs : EventArgs
{
    public Guid Id { get; init; }
    public string SourceName { get; init; } = string.Empty;
    public string? Title { get; init; }
    public string Text { get; init; } = string.Empty;
    public DateTime PublishDate { get; init; }
}
