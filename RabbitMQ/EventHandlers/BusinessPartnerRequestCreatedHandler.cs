using ConversationBackend.Models;
using ConversationBackend.RabbitMQ.Events;
using ConversationBackend.Repositories.RepositoryInterfaces;
using MediatR;
using System.Text.Json;

namespace ConversationBackend.RabbitMQ.EventHandlers
{
    public class BusinessPartnerRequestCreatedHandler : INotificationHandler<BusinessPartnerRequestCreatedEvent>
    {
        private readonly IAMLRepository _amlRepo;

        public BusinessPartnerRequestCreatedHandler(IAMLRepository amlRepo)
        {
            _amlRepo = amlRepo;
        }

        public async Task Handle(BusinessPartnerRequestCreatedEvent businessPartnerRequestCreatedEvent, CancellationToken token)
        {
            Console.WriteLine("Event Data: " + JsonSerializer.Serialize(businessPartnerRequestCreatedEvent));
            BusinessPartnerAMLExtVM res = _amlRepo.screeningRequest(businessPartnerRequestCreatedEvent.businessPartnerAMLVM);
            Console.WriteLine("SCREEN: " + JsonSerializer.Serialize(res));
        }
    }
}
