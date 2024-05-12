namespace NewsService.Core.NewsLoader.Models;

public record NewsInfo(
    string RssUrl,
    string SourceName,
    string? Title,
    DateTime PublishDate);