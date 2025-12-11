using ConversationBackend.Models.DRP;

namespace ConversationBackend.Repositories.RepositoryInterfaces
{
    public interface IDRPSystemRepository
    {
        Task<DRPNicResponseVM> GetDRPResponse(string nicNo);
    }
}
