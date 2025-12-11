namespace ConversationBackend.Models.DRP
{
    public class DRPLoginResponseVM
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? ExpiresIn { get; set; }
        public string? RefreshExpiresIn { get; set; }
        public string? TokenType { get; set; }
        public int? UserId { get; set; }
    }
}
