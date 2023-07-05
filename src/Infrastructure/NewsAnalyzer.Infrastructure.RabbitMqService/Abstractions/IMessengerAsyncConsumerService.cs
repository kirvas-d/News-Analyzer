using NewsAnalyzer.Infrastructure.RabbitMqService.Events;

namespace NewsAnalyzer.Infrastructure.RabbitMqService.Abstractions;

public delegate Task AsyncEventHandler<TMessage>(object? sender, MessageReceivedEventArgs<TMessage> e);
public interface IMessengerAsyncConsumerService<TMessage>
{
    public event AsyncEventHandler<TMessage> Received;

    public void AcknowledgeConsumeMessage(ulong DeliveryTag);
}