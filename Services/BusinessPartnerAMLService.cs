using ConversationBackend.Models;
using ConversationBackend.RabbitMQ;
using ConversationBackend.RabbitMQ.Events;
using ConversationBackend.Repositories.RepositoryInterfaces;
using ConversationBackend.Services.ServiceInterfaces;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ConversationBackend.Services
{
    public class BusinessPartnerAMLService : IBusinessPartnerAMLService
    {
        private readonly IBusinessPartnerAMLRepository _repository;
        private readonly IEventBusPublisher _publisher;
        public BusinessPartnerAMLService(IBusinessPartnerAMLRepository repository, IEventBusPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<BusinessPartner> GetBusinessPartner(int businessPartnerId)
        {
            var res =  await _repository.GetBusinessPartner(businessPartnerId);
            Console.WriteLine(JsonSerializer.Serialize(res));
            return res;
        }

        public async Task<bool> GetRefinitivResponse(BusinessPartnerRequestCreatedEvent request)
        {
            try
            {
                await _publisher.PublishEvent(request);
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
