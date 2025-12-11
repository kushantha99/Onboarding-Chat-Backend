using ConversationBackend.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ConversationBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DRPController : ControllerBase
    {
        private readonly IDRPService _drpService;

        public DRPController(IDRPService drpService)
        {
            _drpService = drpService;
        }


        [HttpGet("{nicNo}")]
        public async Task<IActionResult> GetDRPResponse(string nicNo)
        {
            try
            {
                var res = await _drpService.GetDRPResponse(nicNo);
                if(res == true)
                {
                    return Ok("Event sent to queue");
                }
                return BadRequest("Failed to send event to queue");
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Internal server errorrr: {ex.Message}");
            }
        }
    }
}
