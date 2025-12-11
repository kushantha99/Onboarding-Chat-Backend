using ConversationBackend.Models.DRP;
using MediatR;

namespace ConversationBackend.RabbitMQ.Events
{
    public class DRPRequestCreatedEvent : INotification
    {
        public DRPNicRequestVM dRPNicRequestVM { get; set; }
    }
}
