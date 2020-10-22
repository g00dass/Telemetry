using Microsoft.Extensions.Caching.Memory;

namespace DataLayer
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IMongoClientProvider mongoClientProvider;
        private readonly IMongoDbSettings settings;
        private readonly IMemoryCache cache;

        public UnitOfWorkFactory(
            IMongoClientProvider mongoClientProvider, IMongoDbSettings settings, IMemoryCache cache)
        {
            this.mongoClientProvider = mongoClientProvider;
            this.settings = settings;
            this.cache = cache;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(mongoClientProvider.Client, settings, cache);
        }
    }
}
