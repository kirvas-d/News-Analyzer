﻿using NewsAnalyzer.Application.NewsService;
using NewsAnalyzer.Core.Abstractions;
using NewsAnalyzer.Core.Events;
using NewsAnalyzer.Core.Models;
using NewsAnalyzer.Infrastructure.RabbitMqService.Abstractions;
using static NewsAnalyzer.Application.NewsService.ApplicationNews;

namespace NewsAnalyzer.Application.NerService.Services;

public class BackgroundNerService : BackgroundService
{
    private readonly INerService _nerService;
    private readonly IMessengerConsumerService<NewsLoadedEventArgs> _messengerConsumerService;
    private readonly INamedEntityFormRepository _namedEntityFormRepository;
    private readonly ApplicationNews.ApplicationNewsClient _applicationNewsClient;

    public BackgroundNerService(
        INerService nerService, 
        IMessengerConsumerService<NewsLoadedEventArgs> messengerConsumerService, 
        INamedEntityFormRepository namedEntityFormAsyncRepository, 
        ApplicationNewsClient applicationNewsClient)
    {
        _nerService = nerService;
        _messengerConsumerService = messengerConsumerService;
        _namedEntityFormRepository = namedEntityFormAsyncRepository;
        _applicationNewsClient = applicationNewsClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messengerConsumerService.Received += _messengerConsumerService_Received;
    }

    private void _messengerConsumerService_Received(object? sender, Infrastructure.RabbitMqService.Events.MessageReceivedEventArgs<NewsLoadedEventArgs> e)
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

        foreach (var namedEntityForm in unexsistedNamedEntityForms)
        {
            _namedEntityFormRepository.Add(namedEntityForm);
        }
        //_namedEntityFormRepository.AddRange(unexsistedNamedEntityForms);           
        _messengerConsumerService.AcknowledgeConsumeMessage(e.DeliveryTag);
    }
}
