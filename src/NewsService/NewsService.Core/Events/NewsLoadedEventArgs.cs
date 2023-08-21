namespace NewsService.Core.Events;

public class NewsLoadedEventArgs : EventArgs
{
    public Guid NewsId { get; init; }
}
