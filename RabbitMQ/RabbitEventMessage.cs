namespace ConversationBackend.RabbitMQ
{
    public class RabbitEventMessage
    {
        public string EventType { get; set; }   // e.g. "BusinessPartnerRequestCreated"
        public string Payload { get; set; }     // JSON serialized object
    }
}
