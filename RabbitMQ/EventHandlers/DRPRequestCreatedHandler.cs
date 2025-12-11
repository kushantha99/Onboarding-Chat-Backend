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
            await _repository.GetDRPResponse(dRPRequestCreatedEvent.dRPNicRequestVM.NICNo);
        }
    }
}
