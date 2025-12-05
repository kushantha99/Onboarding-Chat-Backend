using MongoDB.Bson.Serialization.Attributes;

namespace ConversationBackend.Models
{
    [BsonIgnoreExtraElements]
    public class BusinessPartner
    {
        public int BusinessPartnerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
}
