namespace ConversationBackend.Models.DRP
{
    public class MainClientDRPSystemDetailsVM
    {
        public string NIC { get; set; }
        public string Code { get; set; }
        public string Response { get; set; }
        public bool IsRegistered { get; set; }
        public int RequestedUserId { get; set; }
        public DateTime RequestedDateTime { get; set; }
    }
}
