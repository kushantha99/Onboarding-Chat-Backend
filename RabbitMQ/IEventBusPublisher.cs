namespace ConversationBackend.RabbitMQ
{
    public interface IEventBusPublisher
    {
        Task PublishEvent<T>(T eventData);
    }
}
