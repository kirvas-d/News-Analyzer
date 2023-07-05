using NewsAnalyzer.Infrastructure.RabbitMqService.Abstractions;
using NewsAnalyzer.Infrastructure.RabbitMqService.Events;
using NewsAnalyzer.Infrastructure.RabbitMqService.Models;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace NewsAnalyzer.Infrastructure.RabbitMqService.Services;

public class RabbitMqMessengerAsyncConsumerService<TMessage> : RabbitMqMessengerService<TMessage>, IMessengerAsyncConsumerService<TMessage>
{
    private AsyncEventingBasicConsumer? _asyncConsumer;
    private Abstractions.AsyncEventHandler<TMessage>? _asyncReceived;

    public RabbitMqMessengerAsyncConsumerService(RabbitMqMessengerServiceConfiguration configuration) : base(configuration)
    {
    }

    public event Abstractions.AsyncEventHandler<TMessage> Received 
    {
        add
        {
            _asyncReceived += value;
            if (_asyncConsumer == null)
                CreateAsyncEventingBasicConsumer();
        }
        remove
        {
            if (_asyncReceived != null)
                _asyncReceived -= value;
        }
    }

    public void AcknowledgeConsumeMessage(ulong DeliveryTag)
    {
        _channel.BasicAck(DeliveryTag, false); 
    }

    private void CreateAsyncEventingBasicConsumer()
    {
        _asyncConsumer = new AsyncEventingBasicConsumer(_channel);
        _asyncConsumer.Received += Consumer_Received;
        _channel.BasicConsume(queue: _configuration.QueueName, autoAck: false, consumer: _asyncConsumer);
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
    {
        var message = ConvertJsonToMessage<TMessage>(@event);
        await InvokeAsync(_asyncReceived, this, new MessageReceivedEventArgs<TMessage> { Message = message, DeliveryTag = @event.DeliveryTag });
    }

    private async Task InvokeAsync(Abstractions.AsyncEventHandler<TMessage> eventHandler, object sender, MessageReceivedEventArgs<TMessage> e)
    {
        if (eventHandler != null)
        {
            foreach (Abstractions.AsyncEventHandler<TMessage> handlerInstance in eventHandler.GetInvocationList())
            {
                await handlerInstance(sender, e).ConfigureAwait(false);
            }
        }
    }
}
