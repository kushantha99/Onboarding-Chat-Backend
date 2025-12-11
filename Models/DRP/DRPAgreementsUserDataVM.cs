using Newtonsoft.Json;

namespace ConversationBackend.Models.DRP
{
    public class DRPAgreementsUserDataVM
    {
        [JsonProperty("data")]
        public List<AgreementVM> Data { get; set; }
    }
    public class AgreementVM
    {
        [JsonProperty("agreementId")]
        public int AgreementId { get; set; }
    }

}
