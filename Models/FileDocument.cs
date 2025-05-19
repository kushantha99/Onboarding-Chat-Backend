using MongoDB.Bson;

namespace ConversationBackend.Models
{
    public class FileDocument
    {
        public ObjectId Id { get; set; }
        public required string ConversationId { get; set; }
        public required string QuestionId { get; set; }
        public required string Filename { get; set; }
        public required string ContentType { get; set; }
        public required byte[] Data { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
