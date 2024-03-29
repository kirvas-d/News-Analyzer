﻿namespace NlpService.Core.Models;

public class SentimentAnalyzeResult
{
    public string PredictedLabel { get; init; }

    public IReadOnlyDictionary<string, decimal> Scores { get; init; }
}
