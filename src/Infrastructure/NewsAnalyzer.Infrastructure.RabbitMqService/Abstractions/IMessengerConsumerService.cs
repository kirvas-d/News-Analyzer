using NewsAnalyzer.Infrastructure.RabbitMqService.Events;

namespace NewsAnalyzer.Infrastructure.RabbitMqService.Abstractions;

public interface IMessengerConsumerService<TMessage>
{
    event EventHandler<MessageReceivedEventArgs<TMessage>> Received;
}
