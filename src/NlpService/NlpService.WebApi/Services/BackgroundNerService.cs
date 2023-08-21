using NewsAnalyzer.Application.NewsService;
using NewsAnalyzer.Core.Abstractions;
using NewsService.Core.Events;
using NewsService.Core.Models;
using NlpService.Core.Abstractions;
using NlpService.Core.Models;
using RabbitMqService.Abstractions;
using RabbitMqService.Events;
using static NewsAnalyzer.Application.NewsService.ApplicationNews;

namespace NlpService.WebApi.Services;

public class BackgroundNerService : BackgroundService
{
    private readonly INerService _nerService;
    private readonly IMessengerConsumerService<NewsLoadedEventArgs> _messengerConsumerService;
    private readonly INamedEntityFormRepository _namedEntityFormRepository;
    private readonly ApplicationNewsClient _applicationNewsClient;
    private readonly ILogger<BackgroundNerService> _logger;

    public BackgroundNerService(
        INerService nerService,
        IMessengerConsumerService<NewsLoadedEventArgs> messengerConsumerService,
        INamedEntityFormRepository namedEntityFormAsyncRepository,
        ApplicationNewsClient applicationNewsClient,
        ILogger<BackgroundNerService> logger)
    {
        _nerService = nerService;
        _messengerConsumerService = messengerConsumerService;
        _namedEntityFormRepository = namedEntityFormAsyncRepository;
        _applicationNewsClient = applicationNewsClient;
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

            var news = new News(Guid.Parse(newsResponse.Id),
                                newsResponse.SourceName,
                                newsResponse.Title,
                                newsResponse.Text,
                                newsResponse.PublishDate.ToDateTime());
            var namedEntityForms = _nerService.GetNamedEntityFormsFromNews(news);

            var exsistedNamedEntityForms = _namedEntityFormRepository.GetWhere(entity => namedEntityForms
                                                                                             .Select(e => e.Value)
                                                                                             .Contains(entity.Value));

            var unexsistedNamedEntityForms = new List<NamedEntityForm>();
            foreach (var namedEntityForm in namedEntityForms)
            {
                var exsistedNamedEntityForm = exsistedNamedEntityForms?.FirstOrDefault(e => e.Value == namedEntityForm.Value);
                if (exsistedNamedEntityForm != null)
                {
                    exsistedNamedEntityForm.AddNewsId(news.Id);
                    _namedEntityFormRepository.Update(exsistedNamedEntityForm);
                }
                else
                {
                    unexsistedNamedEntityForms.Add(namedEntityForm);
                }
            }

            _namedEntityFormRepository.AddRange(unexsistedNamedEntityForms);
            _messengerConsumerService.AcknowledgeConsumeMessage(e.DeliveryTag);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.ToString());
        }
    }
}