using MongoDB.Bson.Serialization.Attributes;

namespace ConversationBackend.Models
{
    public class Conversation
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string ConversationId { get; set; } = null!;
        public string CurrentQuestionId { get; set; } = null!;
        public Dictionary<string, Question> Questions { get; set; } = null!;
    }
}
