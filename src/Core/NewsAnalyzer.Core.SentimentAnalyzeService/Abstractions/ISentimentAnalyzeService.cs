using NewsAnalyzer.Core.SentimentAnalyzeService.Models;

namespace NewsAnalyzer.Core.SentimentAnalyzeService.Abstractions;

public interface ISentimentAnalyzeService
{
    SentimentAnalyzeResult Predict(string Text);
}
