namespace NewsAnalyzer.Infrastructure.RabbitMqService.Events
{
    public class MessageReceivedEventArgs<TMessage> : EventArgs
    {
        ulong DeliveryTag { get; init; }
        public TMessage Message { get; init;}
    }
}
