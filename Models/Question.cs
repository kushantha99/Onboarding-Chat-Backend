namespace ConversationBackend.Models
{
    public class Question
    {
        public string QuestionId { get; set; } = null!; // Required
        public string QuestionText { get; set; } = null!; // Required
        public string InputType { get; set; } = null!; // Required
        public List<Option>? Options { get; set; } // Nullable, null for text inputs
        public string? Placeholder { get; set; } // Nullable, null for buttons
        public Validation? Validation { get; set; } // Nullable, null for some questions
        public string? NextQuestionId { get; set; } // Nullable, null for some
        public Dictionary<string, Question>? SubQuestion { get; set; } // Nullable, null for most
        public bool RequiresSubmitButton { get; set; }
        public int? LayoutColumn { get; set; } // Nullable, used in Q4
        public string? ValidationKey { get; set; } // Nullable, null for some
        public string? MinDate { get; set; } // Nullable, used in calendar inputs
        public string? MaxDate { get; set; } // Nullable, used in calendar inputs
    }
}