using ConversationBackend.Models;
using ConversationBackend.RabbitMQ;
using ConversationBackend.RabbitMQ.Events;
using ConversationBackend.Services.ServiceInterfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace ConversationBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessPartnerAMLController : Controller
    {
        private readonly IBusinessPartnerAMLService _businessPartnerAMLService;
        private readonly IEventBusPublisher _publisher;

        public BusinessPartnerAMLController(IBusinessPartnerAMLService businessPartnerAMLService, IEventBusPublisher publisher)
        {
            _businessPartnerAMLService = businessPartnerAMLService;
            _publisher = publisher;
        }


        // GET: api/<BusinessPartnerAMLController>
        [HttpGet("{businessPartnerId}")]
        public async Task<BusinessPartner> GetBusinessPartner(int businessPartnerId)
        {
            try
            {
                return await _businessPartnerAMLService.GetBusinessPartner(businessPartnerId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // POST api/<BusinessPartnerAMLController>
        [HttpPost("Initiate")]
        public async Task<IActionResult> GetRefinitivResponse([FromBody] BusinessPartnerRequestCreatedEvent request)
        {
            try
            {
                var res = await _businessPartnerAMLService.GetRefinitivResponse(request);
                if(res == true)
                {
                    return Ok("Event sent to queue");
                }
                return BadRequest("Failed to send event to queue");
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
