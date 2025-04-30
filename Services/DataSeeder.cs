using MongoDB.Driver;
using ConversationBackend.Models;

namespace ConversationBackend.Services
{
    public class DataSeeder
    {
        private readonly MongoDbService _mongoDbService;

        public DataSeeder(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task SeedData()
        {
            try
            {
                var filter = Builders<Conversation>.Filter.Eq(c => c.ConversationId, "8631d9f7-1d59-45d3-9566-c12263800746");
                var existingConversation = await _mongoDbService.Conversations
                    .Find(filter)
                    .FirstOrDefaultAsync();

                // Check if the existing conversation has all expected questions
                bool isIncomplete = existingConversation == null ||
                    existingConversation.Questions == null ||
                    existingConversation.Questions.Count < 10; // Expect 10 questions

                if (isIncomplete)
                {
                    if (existingConversation != null)
                    {
                        Console.WriteLine($"Existing conversation found but incomplete (Questions count: {existingConversation.Questions?.Count ?? 0}). Replacing...");
                        // Delete the incomplete document
                        await _mongoDbService.Conversations.DeleteOneAsync(filter);
                    }
                    else
                    {
                        Console.WriteLine("No existing conversation found. Inserting new conversation...");
                    }

                    var conversation = new Conversation
                    {
                        ConversationId = "8631d9f7-1d59-45d3-9566-c12263800746",
                        CurrentQuestionId = "Q1",
                        Questions = new Dictionary<string, Question>
                        //{
                        //    ["Q1"] = new Question
                        //    {
                        //        QuestionId = "Q1",
                        //        QuestionText = "What is the business partner type?",
                        //        InputType = "buttons",
                        //        Options = new List<Option>
                        //        {
                        //            new Option { Text = "Individual", NextQuestionId = "Q2" },
                        //            new Option { Text = "Corporate", NextQuestionId = "Q3" }
                        //        },
                        //        RequiresSubmitButton = false
                        //    }
                            
                        //}
                    };
                    await _mongoDbService.Conversations.InsertOneAsync(conversation);
                    Console.WriteLine("Conversation inserted successfully!");
                }
                else
                {
                    Console.WriteLine($"Conversation already exists with {existingConversation.Questions.Count} questions.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to seed conversation: {ex.Message}");
                throw;
            }
        }
    }
}