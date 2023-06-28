namespace NewsAnalyzer.Infrastructure.RabbitMqService.Models;

public class RabbitMqMessengerServiceConfiguration
{
    public string HostName { get; init; }

    public string QueueName { get; init; }
}
