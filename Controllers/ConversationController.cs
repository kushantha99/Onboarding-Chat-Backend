using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ConversationBackend.Models;
using ConversationBackend.Services;

namespace ConversationBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public ConversationController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpGet("{conversationId}")]
        public async Task<ActionResult<Conversation>> GetConversation(string conversationId)
        {
            var filter = Builders<Conversation>.Filter.Eq(c => c.ConversationId, conversationId);
            var conversation = await _mongoDbService.Conversations
                .Find(filter)
                .FirstOrDefaultAsync();

            if (conversation == null)
            {
                return NotFound();
            }

            return Ok(conversation);
        }
    }
}