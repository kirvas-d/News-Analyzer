namespace NewsAnalyzer.Core.Models;

public record News(
    string SourceName,
    string? title,
    string Text,
    DateTime PublishDate);
