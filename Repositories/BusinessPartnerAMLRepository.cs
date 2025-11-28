using ConversationBackend.Models;
using ConversationBackend.Repositories.RepositoryInterfaces;
using MongoDB.Driver;

namespace ConversationBackend.Repositories
{
    public class BusinessPartnerAMLRepository : IBusinessPartnerAMLRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<BusinessPartner> _businessPartnerCollection;

        public BusinessPartnerAMLRepository(IConfiguration configuration) 
        {
            try
            {
                var settings = configuration.GetSection("MongoDbSettings");
                var connectionString = settings["ConnectionString"];
                var databaseName = settings["DatabaseName"];
                var client = new MongoClient(connectionString);
                _database = client.GetDatabase(databaseName);

                //Initialize Mongo Collection
                _businessPartnerCollection = _database.GetCollection<BusinessPartner>("businessPartner");
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"MongoDB Atlas connection failed: {ex.Message}");
                throw;
            }
        }

        public async Task<BusinessPartner> GetRefinitivResponse(int businessPartnerId)
        {
            try
            {
                var filter = Builders<BusinessPartner>.Filter.Eq(x => x.BusinessPartnerId, businessPartnerId);
                var result = await _businessPartnerCollection.Find(filter).FirstOrDefaultAsync();
                return result;

            }
            catch(Exception ex)
            {
                Console.WriteLine($"MongoDB query failed: {ex.Message}");
                throw;
            }
        }
    }
}
