using RabbitMqService.Events;

namespace RabbitMqService.Abstractions;

public interface IMessengerConsumerService<TMessage>
{
    public event EventHandler<MessageReceivedEventArgs<TMessage>> Received;

    public void AcknowledgeConsumeMessage(ulong DeliveryTag);
}
