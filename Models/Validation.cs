namespace ConversationBackend.Models
{
    public class Validation
    {
        public bool Required { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
        public string? Pattern { get; set; } // Nullable, as often not used
    }
}