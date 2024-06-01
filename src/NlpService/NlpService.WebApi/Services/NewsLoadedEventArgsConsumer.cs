namespace NlpService.WebApi.Services;

using MassTransit;
using NewsService.Core.NewsLoader.Events;
using NlpService.Core.Abstractions;
using NlpService.Core.Models;
using NlpService.Data.Abstractions;

public class NewsLoadedEventArgsConsumer : IConsumer<NewsLoadedEventArgs>
{
    private readonly INerService _nerService;
    private readonly INlpUnitOfWork _nlpUnitOfWork;
    private readonly ILogger<NewsLoadedEventArgsConsumer> _logger;

    public NewsLoadedEventArgsConsumer(
        INerService nerService,
        INlpUnitOfWork nlpUnitOfWork,
        ILogger<NewsLoadedEventArgsConsumer> logger)
    {
        _nerService = nerService;
        _nlpUnitOfWork = nlpUnitOfWork;
        _logger = logger;
    }

    public Task Consume(ConsumeContext<NewsLoadedEventArgs> context)
    {
        try
        {
            var namedEntityValues = _nerService.GetNamedEntityFormsFromNews(context.Message.Text);
            var news = _nlpUnitOfWork.TextRepository.GetById(context.Message.Id);
            if (news == null)
            {
                news = new Text(context.Message.Id, null);
                _nlpUnitOfWork.TextRepository.Add(news);
            }

            foreach (var value in namedEntityValues)
            {
                var namedEntityForm = _nlpUnitOfWork.NamedEntityFormRepository.GetByValue(value);
                if (namedEntityForm == null)
                {
                    namedEntityForm = new NamedEntityForm(value, new List<Text> { news });
                    _nlpUnitOfWork.NamedEntityFormRepository.Add(namedEntityForm);
                }
                else
                {
                    namedEntityForm.AddNews(news);
                    _nlpUnitOfWork.NamedEntityFormRepository.Update(namedEntityForm);
                }
            }

            _nlpUnitOfWork.SaveChanges();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.ToString());
        }

        return Task.CompletedTask;
    }
}
