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

        public UnitOfWorkFactory(
            IMongoClientProvider mongoClientProvider, IMongoDbSettings settings)
        {
            this.mongoClientProvider = mongoClientProvider;
            this.settings = settings;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(mongoClientProvider.Client, settings);
        }
    }
}
