using NlpService.Core.Models;

namespace NlpService.Core.Abstractions;

public interface ISentimentAnalyzeService
{
    SentimentAnalyzeResult Predict(string Text);
}
