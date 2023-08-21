using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqService.Abstractions;
using RabbitMqService.Events;
using RabbitMqService.Models;

namespace RabbitMqService.Services;

public class RabbitMqMessengerConsumerService<TMessage> : RabbitMqMessengerService<TMessage>, IMessengerConsumerService<TMessage>
{
    private EventingBasicConsumer? _consumer;
    private EventHandler<MessageReceivedEventArgs<TMessage>>? _received;

    public RabbitMqMessengerConsumerService(RabbitMqMessengerServiceConfiguration configuration) : base(configuration)
    {
    }

    public event EventHandler<MessageReceivedEventArgs<TMessage>> Received
    {
        add
        {
            _received += value;
            if (_consumer == null)
                CreateEventingBasicConsumer();

        }
        remove
        {
            if (_received != null)
                _received -= value;
        }
    }

    public void AcknowledgeConsumeMessage(ulong DeliveryTag)
    {
        _channel.BasicAck(DeliveryTag, false);
    }

    private void CreateEventingBasicConsumer()
    {
        _consumer = new EventingBasicConsumer(_channel);
        _consumer.Received += Consumer_Received;
        _channel.BasicConsume(queue: _configuration.QueueName, autoAck: false, consumer: _consumer);
    }

    private void Consumer_Received(object? sender, BasicDeliverEventArgs e)
    {
        var message = ConvertJsonToMessage<TMessage>(e);
        _received.Invoke(sender, new MessageReceivedEventArgs<TMessage> { DeliveryTag = e.DeliveryTag, Message = message });
    }
}
