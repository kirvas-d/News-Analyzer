using NewsAnalyzer.Infrastructure.RabbitMqService.Events;

namespace NewsAnalyzer.Infrastructure.RabbitMqService.Abstractions;

public interface IMessengerConsumerService<TMessage>
{
    public event EventHandler<MessageReceivedEventArgs<TMessage>> Received;

    public void AcknowledgeConsumeMessage(ulong DeliveryTag);
}
