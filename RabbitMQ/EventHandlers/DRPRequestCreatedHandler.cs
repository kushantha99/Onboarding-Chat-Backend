using ConversationBackend.Models.DRP;
using ConversationBackend.RabbitMQ.Events;
using ConversationBackend.Repositories.RepositoryInterfaces;
using MediatR;
using System.Text.Json;

namespace ConversationBackend.RabbitMQ.EventHandlers
{
    public class DRPRequestCreatedHandler : INotificationHandler<DRPRequestCreatedEvent>
    {
        private readonly IDRPSystemRepository _repository;
        public DRPRequestCreatedHandler(IDRPSystemRepository repository)
        {
            _repository = repository;
        }
        public async Task Handle(DRPRequestCreatedEvent dRPRequestCreatedEvent, CancellationToken token)
        {
            Console.WriteLine("DRP Event Data: " + JsonSerializer.Serialize(dRPRequestCreatedEvent));

            try
            {
                DRPNicResponseVM getresult = new DRPNicResponseVM();
                getresult = await _repository.GetDRPResponse(dRPRequestCreatedEvent.dRPNicRequestVM.NICNo);
                Console.WriteLine(JsonSerializer.Serialize(getresult));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("INNER: " + ex.InnerException.Message);
                }
            }
        }
    }
}
