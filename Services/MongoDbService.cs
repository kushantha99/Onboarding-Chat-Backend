using ConversationBackend.Models;
using MongoDB.Driver;

namespace ConversationBackend.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration configuration)
        {
            try
            {
                var settings = configuration.GetSection("MongoDbSettings");
                var connectionString = settings["ConnectionString"];
                var databaseName = settings["DatabaseName"];
                Console.WriteLine($"Attempting to connect to MongoDB Atlas: {connectionString}");
                var client = new MongoClient(connectionString);
                _database = client.GetDatabase(databaseName);
                Console.WriteLine("MongoDB Atlas connection established.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MongoDB Atlas connection failed: {ex.Message}");
                throw;
            }
        }

        public IMongoCollection<Conversation> Conversations => _database.GetCollection<Conversation>("conversations");
    }
}