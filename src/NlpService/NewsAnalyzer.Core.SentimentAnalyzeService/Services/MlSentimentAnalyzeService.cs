using Microsoft.ML;
using Microsoft.ML.Data;
using NewsAnalyzer.Core.SentimentAnalyzeService.Abstractions;
using NewsAnalyzer.Core.SentimentAnalyzeService.Models;

namespace NewsAnalyzer.Core.SentimentAnalyzeService.Services;

public class MlSentimentAnalyzeService : ISentimentAnalyzeService
{
    private readonly PredictionEngine<ModelInput, ModelOutput> _predictionEngine;
    private readonly string[] _labels;
    public MlSentimentAnalyzeService(MlSentimentAnalyzeServiceConfiguration configuration)
    {
        if (!File.Exists(configuration.ModelFilePath))
            throw new FileNotFoundException($"{configuration.ModelFilePath} does not exsist!");

        var mlContext = new MLContext();
        ITransformer trainedModel = mlContext.Model.Load(configuration.ModelFilePath, out var modelSchema);
        _predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(trainedModel);

        var labelBuffer = new VBuffer<ReadOnlyMemory<char>>();
        _predictionEngine.OutputSchema["Score"].Annotations.GetValue("SlotNames", ref labelBuffer);
        _labels = labelBuffer.DenseValues().Select(l => l.ToString()).ToArray();
    }

    public SentimentAnalyzeResult Predict(string Text)
    {
        var result = new ModelOutput();
        _predictionEngine.Predict(new ModelInput { Text = Text, Label = string.Empty }, ref result);
        var scores = _labels.ToDictionary(l => l, l => (decimal)result.Score[Array.IndexOf(_labels, l)]);
            
        return new SentimentAnalyzeResult() { PredictedLabel = result.PredictedLabel, Scores = scores };
    }

    class ModelInput
    {
        public string Text { get; set; }
        public string Label { get; set; }
    }

    class ModelOutput
    {
        public string PredictedLabel { get; set; }

        public float[] Score { get; set; }
    }
}
