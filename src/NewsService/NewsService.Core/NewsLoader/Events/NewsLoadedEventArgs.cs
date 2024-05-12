namespace NewsService.Core.NewsLoader.Events;

public class NewsLoadedEventArgs : EventArgs
{
    public Guid Id { get; init; }
    public string SourceName { get; init; }
    public string? Title { get; init; }
    public string Text { get; init; }
    public DateTime PublishDate { get; init; }
}
