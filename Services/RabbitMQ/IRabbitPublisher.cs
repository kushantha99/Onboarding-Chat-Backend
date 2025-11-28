namespace ConversationBackend.Services.RabbitMQ
{
    public interface IRabbitPublisher
    {
        Task PublishBusinessPartnerRequest(int businessPartnerId);
    }
}
