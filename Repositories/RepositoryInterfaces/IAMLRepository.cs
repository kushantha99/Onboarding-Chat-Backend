using ConversationBackend.Models;

namespace ConversationBackend.Repositories.RepositoryInterfaces
{
    public interface IAMLRepository
    {
        public BusinessPartnerAMLExtVM screeningRequest(BusinessPartnerAMLVM businessPartnerAMLVM);
    }
}
