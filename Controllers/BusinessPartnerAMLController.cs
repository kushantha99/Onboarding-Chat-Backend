using ConversationBackend.Models;
using ConversationBackend.Services.RabbitMQ;
using ConversationBackend.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ConversationBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessPartnerAMLController : Controller
    {
        private readonly IBusinessPartnerAMLService _businessPartnerAMLService;
        private readonly IRabbitPublisher _rabbitPublisher;

        public BusinessPartnerAMLController(IBusinessPartnerAMLService businessPartnerAMLService, IRabbitPublisher rabbitPublisher)
        {
            _businessPartnerAMLService = businessPartnerAMLService;
            _rabbitPublisher = rabbitPublisher;
        }


        // GET: api/<BusinessPartnerAMLController>
        //[HttpGet("{businessPartnerId}")]
        //public async Task<BusinessPartner> GetRefinitivResponse(int businessPartnerId)
        //{
        //    try
        //    {
        //        return await _businessPartnerAMLService.GetRefinitivResponse(businessPartnerId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        [HttpGet("{businessPartnerId}")]
        public IActionResult GetRefinitivResponse(int businessPartnerId)
        {
            _rabbitPublisher.PublishBusinessPartnerRequest(businessPartnerId);

            return Ok(new { message = "Request sent to RabbitMQ" });
        }
        // POST api/<BusinessPartnerAMLController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BusinessPartnerAMLController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BusinessPartnerAMLController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
