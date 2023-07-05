using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using NewsAnalyzer.Infrastructure.RabbitMqService.Models;
using NewsAnalyzer.Infrastructure.RabbitMqService.Abstractions;
using System.Text.Json;
using NewsAnalyzer.Infrastructure.RabbitMqService.Events;

namespace NewsAnalyzer.Infrastructure.RabbitMqService.Services;

public class RabbitMqMessengerService<TMessage> : IMessengerPublishService<TMessage>, IMessengerAsyncConsumerService<TMessage>
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly RabbitMqMessengerServiceConfiguration _configuration;
    private AsyncEventingBasicConsumer? _consumer;
    private Abstractions.AsyncEventHandler<TMessage> _received;

    public event Abstractions.AsyncEventHandler<TMessage> Received 
    {
        add 
        {
            if (_consumer == null)
                CreateAsyncEventingBasicConsumer();
            _received += value;
        }
        remove 
        {
            _received -= value;
        }
    }

    public RabbitMqMessengerService(RabbitMqMessengerServiceConfiguration configuration)
    {
        _configuration = configuration;

        var factory = new ConnectionFactory() { HostName = _configuration.HostName, DispatchConsumersAsync = true };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _configuration.QueueName,
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);

    }

    public void PublishMessage(TMessage message)
    {
        if (message == null)
        {
            throw new NullReferenceException();
        }

        var jsonString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonString);

        _channel.BasicPublish(exchange: "",
                             routingKey: _configuration.QueueName,
                             basicProperties: null,
                             body: body);
        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }

    private void CreateAsyncEventingBasicConsumer() 
    {
        _consumer = new AsyncEventingBasicConsumer(_channel);
        _consumer.Received += Consumer_Received;
        _channel.BasicConsume(queue: _configuration.QueueName, autoAck: false, consumer: _consumer);
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
    {
        var jsonString = Encoding.UTF8.GetString(@event.Body.ToArray());
        TMessage? message = JsonSerializer.Deserialize<TMessage>(jsonString);
        await InvokeAsync(_received, this, new MessageReceivedEventArgs<TMessage> { Message = message, DeliveryTag = @event.DeliveryTag});
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

    public void AcknowledgeConsumeMessage(ulong DeliveryTag)
    {
        _channel.BasicAck(DeliveryTag, false);
    }
}
