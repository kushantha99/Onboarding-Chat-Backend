using ConversationBackend.Models.DRP;
using ConversationBackend.RabbitMQ;
using ConversationBackend.RabbitMQ.Events;
using ConversationBackend.Repositories.RepositoryInterfaces;
using ConversationBackend.Services.ServiceInterfaces;

namespace ConversationBackend.Services
{
    public class DRPService : IDRPService
    {
        private readonly IEventBusPublisher _publisher;
        private readonly IDRPSystemRepository _systemRepository;
        public DRPService(IEventBusPublisher publisher, IDRPSystemRepository dRPSystemRepository) 
        {
            _publisher = publisher;
            _systemRepository = dRPSystemRepository;
        }

        public async Task<bool> GetDRPResponse(string nicNo)
        {
            try
            {
                DRPNicRequestVM dRPNicRequestVM = new DRPNicRequestVM();
                dRPNicRequestVM.NICNo = nicNo;

                DRPRequestCreatedEvent dRPRequestCreatedEvent = new DRPRequestCreatedEvent();
                dRPRequestCreatedEvent.dRPNicRequestVM = dRPNicRequestVM;

                await _publisher.PublishEvent(dRPRequestCreatedEvent);
                //await _systemRepository.GetDRPResponse("200006903346");

                return true;
            }
            catch 
            {
                throw;
            }
        }
    }
}
