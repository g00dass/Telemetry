namespace DataLayer
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IMongoClientProvider mongoClientProvider;

        public UnitOfWorkFactory(
            IMongoClientProvider mongoClientProvider)
        {
            this.mongoClientProvider = mongoClientProvider;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(mongoClientProvider.Client);
        }
    }
}
