namespace NlpService.Core.SentimentAnalyzeService.Tests;

using NlpService.Core.Abstractions;
using NlpService.SentimentAnalyzeService.Models;
using NlpService.SentimentAnalyzeService.Services;

public class SentimentAnalyzeServiceTest
{
    private readonly ISentimentAnalyzeService _service;
    public SentimentAnalyzeServiceTest()
    {
        _service = new MlSentimentAnalyzeService(new MlSentimentAnalyzeServiceConfiguration() { ModelFilePath = "model.zip" });
    }

    [Fact]
    public void TestPredict()
    {
        var text = "Сразу после объявления Центробанка (ЦБ) о проведении во вторник, 15 августа, внеочередного заседания совета директоров, на котором будет принято решение о ключевой ставке, курс рубля перешел к резкому росту.";
        var result = _service.Predict(text);

        Assert.Equal("Negative", result.PredictedLabel);
    }
}
