using NewsAnalyzer.Application.NewsService;
using NewsService.Core.Events;
using NlpService.Core.Abstractions;
using NlpService.Core.Models;
using NlpService.Data.Abstractions;
using RabbitMqService.Abstractions;
using RabbitMqService.Events;
using static NewsAnalyzer.Application.NewsService.ApplicationNews;
using News = NlpService.Core.Models.News;

namespace NlpService.WebApi.Services;

public class BackgroundNlpService : BackgroundService
{
    private readonly INerService _nerService;
    private readonly ISentimentAnalyzeService _sentimentAnalyzeService;
    private readonly IMessengerConsumerService<NewsLoadedEventArgs> _messengerConsumerService;
    private readonly ApplicationNewsClient _applicationNewsClient;
    private readonly INlpUnitOfWork _nlpUnitOfWork;
    private readonly ILogger<BackgroundNlpService> _logger;

    public BackgroundNlpService(
        INerService nerService,
        ISentimentAnalyzeService sentimentAnalyzeService,
        IMessengerConsumerService<NewsLoadedEventArgs> messengerConsumerService,
        ApplicationNewsClient applicationNewsClient,
        INlpUnitOfWork nlpUnitOfWork,
        ILogger<BackgroundNlpService> logger)
    {
        _nerService = nerService;
        _sentimentAnalyzeService = sentimentAnalyzeService;
        _messengerConsumerService = messengerConsumerService;
        _applicationNewsClient = applicationNewsClient;
        _nlpUnitOfWork = nlpUnitOfWork;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messengerConsumerService.Received += _messengerConsumerService_Received;
    }

    private void _messengerConsumerService_Received(object? sender, MessageReceivedEventArgs<NewsLoadedEventArgs> e)
    {
        try
        {
            var newsResponse = _applicationNewsClient.GetNews(new NewsRequest { Id = e.Message.NewsId.ToString() });    
            var namedEntityValues = _nerService.GetNamedEntityFormsFromNews(newsResponse.Text);
            var sentimentResult = _sentimentAnalyzeService.Predict(newsResponse.Text);

            var newsId = Guid.Parse(newsResponse.Id);
            var news = _nlpUnitOfWork.NewsRepository.GetById(newsId);
            if (news == null) 
            {
                news = new News(newsId, sentimentResult);
                _nlpUnitOfWork.NewsRepository.Add(news);
            }

            foreach (var value in namedEntityValues) 
            {
                var namedEntityForm = _nlpUnitOfWork.NamedEntityFormRepository.GetByValue(value);
                if (namedEntityForm == null)
                {
                    namedEntityForm = new NamedEntityForm(value, new List<News> { news });
                    _nlpUnitOfWork.NamedEntityFormRepository.Add(namedEntityForm);
                }
                else 
                {
                    namedEntityForm.AddNews(news);
                    _nlpUnitOfWork.NamedEntityFormRepository.Update(namedEntityForm);
                }
            }

            _nlpUnitOfWork.SaveChanges();
            _messengerConsumerService.AcknowledgeConsumeMessage(e.DeliveryTag);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.ToString());
        }
    }
}
