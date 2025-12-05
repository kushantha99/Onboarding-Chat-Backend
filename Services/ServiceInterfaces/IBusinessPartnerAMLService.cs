using ConversationBackend.Models;
using ConversationBackend.RabbitMQ.Events;

namespace ConversationBackend.Services.ServiceInterfaces
{
    public interface IBusinessPartnerAMLService
    {
        Task<BusinessPartner> GetBusinessPartner(int businessPartnerId);
        Task<bool> GetRefinitivResponse(BusinessPartnerRequestCreatedEvent request);
    }
}
