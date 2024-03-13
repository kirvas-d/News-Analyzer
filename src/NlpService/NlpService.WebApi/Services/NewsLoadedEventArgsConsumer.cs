using MassTransit;
using NewsService.Core.Events;
using NlpService.Core.Abstractions;
using NlpService.Core.Models;
using NlpService.Data.Abstractions;
using RabbitMqService.Abstractions;

namespace NlpService.WebApi.Services;

public class NewsLoadedEventArgsConsumer : IConsumer<NewsLoadedEventArgs>
{
    private readonly INerService _nerService;
    private readonly ISentimentAnalyzeService _sentimentAnalyzeService;
    //private readonly IMessengerConsumerService<NewsLoadedEventArgs> _messengerConsumerService;
    //private readonly ApplicationNewsClient _applicationNewsClient;
    private readonly INlpUnitOfWork _nlpUnitOfWork;
    private readonly ILogger<NewsLoadedEventArgsConsumer> _logger;



    public NewsLoadedEventArgsConsumer(
        INerService nerService, 
        ISentimentAnalyzeService sentimentAnalyzeService, 
        IMessengerConsumerService<NewsLoadedEventArgs> messengerConsumerService, 
        //ApplicationNewsClient applicationNewsClient, 
        INlpUnitOfWork nlpUnitOfWork, 
        ILogger<NewsLoadedEventArgsConsumer> logger)
    {
        _nerService = nerService;
        _sentimentAnalyzeService = sentimentAnalyzeService;
        //_messengerConsumerService = messengerConsumerService;
        //_applicationNewsClient = applicationNewsClient;
        _nlpUnitOfWork = nlpUnitOfWork;
        _logger = logger;
    }

    public Task Consume(ConsumeContext<NewsLoadedEventArgs> context)
    {
        try
        {
            //var newsResponse = _applicationNewsClient.GetNews(new NewsRequest { Id = e.Message.NewsId.ToString() });
            var namedEntityValues = _nerService.GetNamedEntityFormsFromNews(context.Message.Text);
            var sentimentResult = _sentimentAnalyzeService.Predict(context.Message.Text);

            //var newsId = Guid.Parse(context.Message.Id);
            var news = _nlpUnitOfWork.NewsRepository.GetById(context.Message.Id);
            if (news == null)
            {
                news = new News(context.Message.Id, sentimentResult);
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
            //_messengerConsumerService.AcknowledgeConsumeMessage(e.DeliveryTag);           
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.ToString());
        }

        return Task.CompletedTask;
    }
}
