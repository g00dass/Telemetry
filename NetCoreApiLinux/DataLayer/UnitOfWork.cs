using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DataLayer
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task SaveChanges();
        IClientSessionHandle Session { get; }
    }

    public class UnitOfWork : IUnitOfWork
    {
        public IClientSessionHandle Session { get; }

        public UnitOfWork(IMongoClient mongoClient)
        {
            Session = mongoClient.StartSession();
        }

        public async Task SaveChanges()
        {
            try
            {
                await Session.CommitTransactionAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await Session.AbortTransactionAsync();
            }
        }

        public async ValueTask DisposeAsync()
        {
            await SaveChanges();
            Session?.Dispose();
        }
    }
}
