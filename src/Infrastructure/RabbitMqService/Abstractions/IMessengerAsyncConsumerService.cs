using RabbitMqService.Events;

namespace RabbitMqService.Abstractions;

public delegate Task AsyncEventHandler<TMessage>(object? sender, MessageReceivedEventArgs<TMessage> e);
public interface IMessengerAsyncConsumerService<TMessage>
{
    public event AsyncEventHandler<TMessage> Received;

    public void AcknowledgeConsumeMessage(ulong DeliveryTag);
}