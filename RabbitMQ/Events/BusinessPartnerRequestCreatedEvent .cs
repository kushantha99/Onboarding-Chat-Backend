using ConversationBackend.Models;
using MediatR;

namespace ConversationBackend.RabbitMQ.Events
{
    public class BusinessPartnerRequestCreatedEvent : INotification
    {
        public BusinessPartnerAMLVM businessPartnerAMLVM { get; set; }
        public BusinessPartnerAMLExtVM businessPartnerAMLExtVM { get; set; }
    }
}