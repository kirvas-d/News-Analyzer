using NlpService.SentimentAnalyzeService.Models;

namespace NlpService.SentimentAnalyzeService.Abstractions;

public interface ISentimentAnalyzeService
{
    SentimentAnalyzeResult Predict(string Text);
}
