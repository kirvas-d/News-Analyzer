namespace NewsService.Core.Models;

public record NewsInfo(
    string RssUrl,
    string SourceName,
    string? Title,
    DateTime PublishDate);