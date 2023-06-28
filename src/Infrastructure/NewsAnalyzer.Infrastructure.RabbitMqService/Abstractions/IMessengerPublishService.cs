namespace NewsAnalyzer.Infrastructure.RabbitMqService.Abstractions;

public interface IMessengerPublishService<TMessage>
{
    void PublishMessage(TMessage message);
}
