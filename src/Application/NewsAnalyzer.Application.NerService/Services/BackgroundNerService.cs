using Grpc.Net.Client;
using NewsAnalyzer.Application.NewsService;
using NewsAnalyzer.Core.Abstractions;
using NewsAnalyzer.Core.Events;
using NewsAnalyzer.Core.Models;
using NewsAnalyzer.Infrastructure.RabbitMqService.Abstractions;

namespace NewsAnalyzer.Application.NerService.Services
{
    public class BackgroundNerService : BackgroundService
    {
        private readonly INerService _nerService;
        private readonly IMessengerAsyncConsumerService<NewsLoadedEventArgs> _messengerAsyncConsumerService;
        private readonly INamedEntityFormAsyncRepository _namedEntityFormAsyncRepository;
        private readonly ApplicationNews.ApplicationNewsClient _applicationNewsClient;

        public BackgroundNerService(INerService nerService, IMessengerAsyncConsumerService<NewsLoadedEventArgs> messengerAsyncConsumerService, INamedEntityFormAsyncRepository namedEntityFormAsyncRepository)
        {
            _nerService = nerService;
            _messengerAsyncConsumerService = messengerAsyncConsumerService;
            _namedEntityFormAsyncRepository = namedEntityFormAsyncRepository;
            _applicationNewsClient = new ApplicationNews.ApplicationNewsClient(GrpcChannel.ForAddress("https://localhost:63405"));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _messengerAsyncConsumerService.Received += _messengerAsyncConsumerService_Received;
        }

        private async Task _messengerAsyncConsumerService_Received(object? sender, Infrastructure.RabbitMqService.Events.MessageReceivedEventArgs<NewsLoadedEventArgs> e)
        {
            var newsResponse = await _applicationNewsClient.GetNewsAsync(new NewsRequest { Id = e.Message.NewsId.ToString() });
            var news = new News(Guid.Parse(newsResponse.Id),
                                newsResponse.SourceName,
                                newsResponse.Title,
                                newsResponse.Text,
                                newsResponse.PublishDate.ToDateTime());
            var namedEntityForms = _nerService.GetNamedEntityFormsFromNews(news);
            //var exsistedNamedEntityForms = await _namedEntityFormAsyncRepository.GetWhereAsync(entity => namedEntityForms
            //                                                                                                 .Select(e => e.Value)
            //                                                                                                 .Contains(entity.Value));
            var exsistedNamedEntityForms = await _namedEntityFormAsyncRepository.GetByValueAsync(namedEntityForms.Select(e => e.Value));

            var unexsistedNamedEntityForms = new List<NamedEntityForm>();
            foreach (var namedEntityForm in namedEntityForms)
            {
                var exsistedNamedEntityForm = exsistedNamedEntityForms?.FirstOrDefault(e => e.Value == namedEntityForm.Value);
                if (exsistedNamedEntityForm != null)
                {
                    exsistedNamedEntityForm.AddNewsId(news.Id);
                    await _namedEntityFormAsyncRepository.UpdateAsync(exsistedNamedEntityForm);
                }
                else 
                {
                    unexsistedNamedEntityForms.Add(namedEntityForm);
                }
            }

            foreach (var namedEntityForm in unexsistedNamedEntityForms) 
            {
                await _namedEntityFormAsyncRepository.AddAsync(namedEntityForm);
            }
            //await _namedEntityFormAsyncRepository.AddRangeAsync(unexsistedNamedEntityForms);
            _messengerAsyncConsumerService.AcknowledgeConsumeMessage(e.DeliveryTag);
        }
    }
}
