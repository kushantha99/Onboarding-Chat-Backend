using Newtonsoft.Json;

namespace ConversationBackend.Models.DRP
{
    public class DRPSubscriptionRequestVM
    {
        [JsonProperty("subscriptions")]
        public List<KeysVM> Subscriptions { get; set; }
    }
    public class KeysVM
    {
        [JsonProperty("subscriptionId")]
        public int SubscriptionId { get; set; }
    }
}
