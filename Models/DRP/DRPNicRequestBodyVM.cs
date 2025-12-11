using Newtonsoft.Json;

namespace ConversationBackend.Models.DRP
{
    public class DRPNicRequestBodyVM
    {
        [JsonProperty("subscriptionId")]
        public int SubscriptionId { get; set; }

        [JsonProperty("keys")]
        public DRPKeysVM Keys { get; set; }

        [JsonProperty("consent")]
        public bool Consent { get; set; }
    }
    public class DRPKeysVM
    {
        [JsonProperty("nic")]
        public string Nic { get; set; }
    }
}
