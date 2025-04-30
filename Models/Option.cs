namespace ConversationBackend.Models
{
    public class Option
    {
        public string Text { get; set; } = null!; // Required
        public string? Value { get; set; } // Nullable, often null in your data
        public string? NextQuestionId { get; set; } // Nullable, can be null (e.g., "No, I'm done")
    }
}