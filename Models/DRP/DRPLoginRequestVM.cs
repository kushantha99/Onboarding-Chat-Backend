using System.Text.Json.Serialization;

namespace ConversationBackend.Models.DRP
{
    public class DRPLoginRequestVM
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("clientSecret")]
        public string ClientSecret { get; set; }
    }
}
