namespace ConversationBackend.Services.ServiceInterfaces
{
    public interface IDRPService
    {
        Task<bool> GetDRPResponse(string nicNo);
    }
}
