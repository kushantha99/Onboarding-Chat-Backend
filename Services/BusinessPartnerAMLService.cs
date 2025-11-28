using ConversationBackend.Models;
using ConversationBackend.Repositories.RepositoryInterfaces;
using ConversationBackend.Services.ServiceInterfaces;
using System.Text.Json;

namespace ConversationBackend.Services
{
    public class BusinessPartnerAMLService : IBusinessPartnerAMLService
    {
        private readonly IBusinessPartnerAMLRepository _repository;
        public BusinessPartnerAMLService(IBusinessPartnerAMLRepository repository)
        {
            _repository = repository;
        }

        public async Task<BusinessPartner> GetRefinitivResponse(int businessPartnerId)
        {
            var res =  await _repository.GetRefinitivResponse(businessPartnerId);
            Console.WriteLine(JsonSerializer.Serialize(res));
            return res;
        }
    }
}
