using ConversationBackend.Models;

namespace ConversationBackend.Repositories.RepositoryInterfaces
{
    public interface IBusinessPartnerAMLRepository
    {
        Task<BusinessPartner> GetRefinitivResponse(int businessPartnerId);
    }
}
