using ConversationBackend.Models;

namespace ConversationBackend.Services.ServiceInterfaces
{
    public interface IBusinessPartnerAMLService
    {
        Task<BusinessPartner> GetRefinitivResponse(int businessPartnerId);
    }
}
