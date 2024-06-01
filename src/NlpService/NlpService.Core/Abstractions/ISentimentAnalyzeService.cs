namespace NlpService.Core.Abstractions;

using NlpService.Core.Models;

public interface ISentimentAnalyzeService
{
    SentimentAnalyzeResult Predict(string Text);
}
