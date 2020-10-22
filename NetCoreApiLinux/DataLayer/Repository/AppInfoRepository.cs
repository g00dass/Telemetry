using System;
using System.Threading.Tasks;
using DataLayer.Dbo;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace DataLayer.Repository
{
    public interface IAppInfoRepository
    {
        public Task<AppInfoDbo[]> GetAllAsync();
        public Task<AppInfoDbo> FindAsync(string id);
        public Task AddOrUpdateAsync(AppInfoDbo dbo);
        Task DeleteByIdAsync(string id);
    }

    public class AppInfoRepository : IAppInfoRepository
    {
        private readonly IClientSessionHandle session;
        private readonly IMemoryCache cache;
        private readonly IMongoDatabase db;

        public AppInfoRepository(IMongoDatabase db, IClientSessionHandle session, IMemoryCache cache)
        {
            this.session = session;
            this.cache = cache;
            this.db = db;
        }

        public async Task<AppInfoDbo[]> GetAllAsync()
        {
            return (await AppInfos
                    .WithReadPreference(ReadPreference.SecondaryPreferred)
                    .FindAsync(session, _ => true)
                    .ConfigureAwait(false))
                .ToList()
                .ToArray();
        }

        public async Task<AppInfoDbo> FindAsync(string id)
        {
            var filterEq = Builders<AppInfoDbo>.Filter.Eq(x => x.Id, id);

            return (await AppInfos
                    .WithReadPreference(ReadPreference.SecondaryPreferred)
                    .FindAsync(session, filterEq)
                    .ConfigureAwait(false))
                .SingleOrDefault();
        }

        public async Task AddOrUpdateAsync(AppInfoDbo dbo)
        {
            var filterEq = Builders<AppInfoDbo>.Filter.Eq(x => x.Id, dbo.Id);

            dbo.LastUpdatedAt = DateTimeOffset.Now;;

            await AppInfos
                .WithWriteConcern(WriteConcern.WMajority)
                .FindOneAndReplaceAsync(session, filterEq, dbo, new FindOneAndReplaceOptions<AppInfoDbo> { IsUpsert = true })
                .ConfigureAwait(false);
        }

        public Task DeleteByIdAsync(string id)
        {
            var filterEq = Builders<AppInfoDbo>.Filter.Eq(x => x.Id, id);

            return AppInfos
                .WithWriteConcern(WriteConcern.WMajority)
                .DeleteOneAsync(session, filterEq);
        }

        private IMongoCollection<AppInfoDbo> AppInfos => db.GetCollection<AppInfoDbo>("AppInfos");
    }
}
