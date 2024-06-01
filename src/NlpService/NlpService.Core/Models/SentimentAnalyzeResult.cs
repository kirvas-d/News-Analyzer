namespace NlpService.Core.Models;

public class SentimentAnalyzeResult
{
    public string PredictedLabel { get; init; } = string.Empty;

    public IReadOnlyDictionary<string, decimal> Scores { get; init; } = new Dictionary<string, decimal>();
}
