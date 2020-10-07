using MongoDB.Driver;

namespace DataLayer
{
    public interface IMongoDbProvider
    {
        public IMongoDatabase Db { get; }
    }

    public class MongoDbProvider : IMongoDbProvider
    {
        public IMongoDatabase Db { get; }

        public MongoDbProvider(IMongoDbSettings settings)
        {
            Db = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
        }
    }
}
