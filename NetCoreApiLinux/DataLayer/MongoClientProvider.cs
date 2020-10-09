using MongoDB.Driver;

namespace DataLayer
{
    public interface IMongoClientProvider
    {
        public IMongoClient Client { get; }
    }

    public class MongoClientProvider : IMongoClientProvider
    {
        public IMongoClient Client { get; }

        public MongoClientProvider(IMongoDbSettings settings)
        {
            Client = new MongoClient(settings.ConnectionString);
        }
    }
}
