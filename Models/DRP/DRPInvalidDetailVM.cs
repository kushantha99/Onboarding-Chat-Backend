using Newtonsoft.Json;

namespace ConversationBackend.Models.DRP
{
    public class DRPInvalidDetailVM
    {
        [JsonProperty("code")]
        public string? Code { get; set; }
        [JsonProperty("message")]
        public string? Message { get; set; }
    }
}
