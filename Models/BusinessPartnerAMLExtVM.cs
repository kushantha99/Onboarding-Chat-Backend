namespace ConversationBackend.Models
{
    public class BusinessPartnerAMLExtVM
    {
        #region Public Properties
        public int BusinessPartnerId { get; set; }
        public int? ReferenceId { get; set; }
        public int? IsSuccess { get; set; }
        public string CaseId { get; set; }
        public int? IsNeedToBlock { get; set; }
        public int? IsWalletCustomer { get; set; }
        public string? Result { get; set; }
        #endregion
    }
}
