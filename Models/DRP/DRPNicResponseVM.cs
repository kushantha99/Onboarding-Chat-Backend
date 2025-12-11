using Newtonsoft.Json;

namespace ConversationBackend.Models.DRP
{
    public class DRPNicResponseVM
    {
        [JsonProperty("idinformation")]
        public List<DRPNICInformationVM> Idinformation { get; set; }
    }
}
