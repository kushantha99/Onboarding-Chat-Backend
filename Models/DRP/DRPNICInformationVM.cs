using Newtonsoft.Json;

namespace ConversationBackend.Models.DRP
{
    public class DRPNICInformationVM
    {
        [JsonProperty("frontImage")]
        public string? FrontImage { get; set; }

        [JsonProperty("photoImage")]
        public string? PhotoImage { get; set; }

        [JsonProperty("imageRecordAvailable")]
        public bool? ImageRecordAvailable { get; set; }

        [JsonProperty("contactPhoneNo")]
        public string? ContactPhoneNo { get; set; }

        [JsonProperty("gender")]
        public string? Gender { get; set; }

        [JsonProperty("issueDate")]
        public string? IssueDate { get; set; }

        [JsonProperty("previousIDCardNo")]
        public string? PreviousIDCardNo { get; set; }

        [JsonProperty("latestIDCardNo")]
        public string? LatestIDCardNo { get; set; }

        [JsonProperty("prePrintedNumber")]
        public string? PrePrintedNumber { get; set; }

        [JsonProperty("nicStatus")]
        public string? NicStatus { get; set; }

        [JsonProperty("dateOfBirth")]
        public string? DateOfBirth { get; set; }

        [JsonProperty("backImage")]
        public string? BackImage { get; set; }

        [JsonProperty("idCardNumber")]
        public string? IdCardNumber { get; set; }

        [JsonProperty("professionSinhala")]
        public string? ProfessionSinhala { get; set; }

        [JsonProperty("professionEnglish")]
        public string? ProfessionEnglish { get; set; }

        [JsonProperty("professionTamil")]
        public string? ProfessionTamil { get; set; }

        [JsonProperty("fullNameEnglish")]
        public string? FullNameEnglish { get; set; }

        [JsonProperty("fullNameTamil")]
        public string? FullNameTamil { get; set; }

        [JsonProperty("fullNameSinhala")]
        public string? FullNameSinhala { get; set; }

        [JsonProperty("placeOfBirthSinhala")]
        public string? PlaceOfBirthSinhala { get; set; }

        [JsonProperty("placeOfBirthTamil")]
        public string? PlaceOfBirthTamil { get; set; }

        [JsonProperty("placeOfBirthEnglish")]
        public string? PlaceOfBirthEnglish { get; set; }

        [JsonProperty("otherNamesSinhala")]
        public string? OtherNamesSinhala { get; set; }

        [JsonProperty("otherNamesTamil")]
        public string? OtherNamesTamil { get; set; }

        [JsonProperty("otherNamesEnglish")]
        public string? OtherNamesEnglish { get; set; }

        [JsonProperty("addressLine1English")]
        public string? AddressLine1English { get; set; }

        [JsonProperty("addressLine2English")]
        public string? AddressLine2English { get; set; }

        [JsonProperty("addressLine3English")]
        public string? AddressLine3English { get; set; }

        [JsonProperty("addressLine4English")]
        public string? AddressLine4English { get; set; }

        [JsonProperty("addressLine1Sinhala")]
        public string? AddressLine1Sinhala { get; set; }

        [JsonProperty("addressLine2Sinhala")]
        public string? AddressLine2Sinhala { get; set; }

        [JsonProperty("addressLine3Sinhala")]
        public string? AddressLine3Sinhala { get; set; }

        [JsonProperty("addressLine4Sinhala")]
        public string? AddressLine4Sinhala { get; set; }

        [JsonProperty("addressLine1Tamil")]
        public string? AddressLine1Tamil { get; set; }

        [JsonProperty("addressLine2Tamil")]
        public string? AddressLine2Tamil { get; set; }

        [JsonProperty("addressLine3Tamil")]
        public string? AddressLine3Tamil { get; set; }

        [JsonProperty("addressLine4Tamil")]
        public string? AddressLine4Tamil { get; set; }
    }
}
