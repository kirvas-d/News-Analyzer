namespace NewsAnalyzer.Infrastructure.RabbitMqService.Events
{
    public class MessageReceivedEventArgs<TMessage> : EventArgs
    {
        public ulong DeliveryTag { get; init; }
        public TMessage Message { get; init;}
    }
}
