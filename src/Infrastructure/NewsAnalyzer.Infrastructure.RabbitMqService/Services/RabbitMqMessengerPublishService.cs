using NewsAnalyzer.Infrastructure.RabbitMqService.Abstractions;
using NewsAnalyzer.Infrastructure.RabbitMqService.Models;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;

namespace NewsAnalyzer.Infrastructure.RabbitMqService.Services;

public class RabbitMqMessengerPublishService<TMessage> : RabbitMqMessengerService<TMessage>, IMessengerPublishService<TMessage>
{
    public RabbitMqMessengerPublishService(RabbitMqMessengerServiceConfiguration configuration) : base(configuration)
    {
    }

    public void PublishMessage(TMessage message)
    {
        var jsonString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonString);

        _channel.BasicPublish(exchange: "",
                              routingKey: _configuration.QueueName,
                              basicProperties: null,
                              body: body);
        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
    }
}
