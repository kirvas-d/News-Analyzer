using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client.Events;
using RabbitMqService.Models;

namespace RabbitMqService.Services;

public class RabbitMqMessengerService<TMessage> : IDisposable
{
    private readonly IConnection _connection;
    protected readonly IModel _channel;
    protected readonly RabbitMqMessengerServiceConfiguration _configuration;

    public RabbitMqMessengerService(RabbitMqMessengerServiceConfiguration configuration)
    {
        _configuration = configuration;

        var factory = new ConnectionFactory() { HostName = _configuration.HostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        if (!string.IsNullOrWhiteSpace(_configuration.ExcangeName))
            _channel.ExchangeDeclare(exchange: _configuration.ExcangeName, type: ExchangeType.Fanout);

        if (!string.IsNullOrWhiteSpace(_configuration.QueueName))
        {
            _channel.QueueDeclare(queue: _configuration.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: _configuration.QueueName, exchange: _configuration.ExcangeName, routingKey: string.Empty);
        }
        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }

    protected TMessage? ConvertJsonToMessage<TMessage>(BasicDeliverEventArgs @event)
    {
        var jsonString = Encoding.UTF8.GetString(@event.Body.ToArray());
        return JsonSerializer.Deserialize<TMessage>(jsonString);
    }
}