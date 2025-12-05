namespace ConversationBackend.Models
{
    public class BusinessPartnerAMLVM
    {
        #region Public Properties
        public int BusinessPartnerId { get; set; }
        public string Name { get; set; }
        public bool IsIndividual { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Country { get; set; }
        public string? BirthPlace { get; set; }
        public string? Nationality { get; set; }
        public string? RegisteredCountry { get; set; }
        public int? ReferenceId { get; set; }
        public string CaseId { get; set; }
        public int? IsSuccess { get; set; }
        public int? IsWalletCustomer { get; set; }
        #endregion
    }
}
