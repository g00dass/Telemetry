using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Dbo;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace DataLayer.Repository
{
    public interface IStatisticsEventTypeRepository
    {
        Task<Dictionary<string, StatisticsEventTypeDbo>> GetAllAsync();
        Task<StatisticsEventTypeDbo[]> FindByNameAsync(string name);
        Task AddOrUpdateAsync(StatisticsEventTypeDbo[] dbos);
        Task DeleteByNameAsync(string name);
    }

    public class StatisticsEventTypeRepository : IStatisticsEventTypeRepository
    {
        private readonly IClientSessionHandle session;
        private readonly IMongoDatabase db;
        private IMemoryCache cache;

        public StatisticsEventTypeRepository(IMongoDatabase db, IClientSessionHandle session, IMemoryCache cache)
        {
            this.session = session;
            this.db = db;
            this.cache = cache;
        }

        public async Task<Dictionary<string, StatisticsEventTypeDbo>> GetAllAsync()
        {
            return await cache.GetOrCreateAsync(
                nameof(GetAllAsync),
                async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(1);
                    
                    return (await GetAllWithoutCacheAsync().ConfigureAwait(false))
                        .ToDictionary(x => x.Name, x => x);
                });
    }

        public async Task<StatisticsEventTypeDbo[]> FindByNameAsync(string name)
        {
            var filterEq = Builders<StatisticsEventTypeDbo>.Filter.Eq(x => x.Name, name);

            return (await Types
                    .WithReadPreference(ReadPreference.SecondaryPreferred)
                    .FindAsync(session, filterEq)
                    .ConfigureAwait(false))
                .ToList()
                .ToArray();
        }

        public async Task AddOrUpdateAsync(StatisticsEventTypeDbo[] dbos)
        {
            foreach (var item in dbos)
                await Types
                    .WithWriteConcern(WriteConcern.WMajority)
                    .ReplaceOneAsync(
                        session,
                        Builders<StatisticsEventTypeDbo>.Filter.Eq(x => x.Name, item.Name),
                        item,
                        new ReplaceOptions {IsUpsert = true});

            await UpdateCache();
        }

        public Task DeleteByNameAsync(string name)
        {
            var filterEq = Builders<StatisticsEventTypeDbo>.Filter.Eq(x => x.Name, name);

            return Types
                .WithWriteConcern(WriteConcern.WMajority)
                .DeleteOneAsync(session, filterEq);
        }

        private IMongoCollection<StatisticsEventTypeDbo> Types => db.GetCollection<StatisticsEventTypeDbo>("StatisticsEventTypes");

        private async Task<StatisticsEventTypeDbo[]> GetAllWithoutCacheAsync()
        {
            return (await Types
                    .WithReadPreference(ReadPreference.SecondaryPreferred)
                    .FindAsync(session, _ => true)
                    .ConfigureAwait(false))
                .ToList()
                .ToArray();
        }

        private async Task UpdateCache()
        {
            var allTypes = (await GetAllWithoutCacheAsync().ConfigureAwait(false))
                .ToDictionary(x => x.Name, x => x);

            using var entry = cache.CreateEntry(nameof(GetAllAsync)).SetValue(allTypes).SetSlidingExpiration(TimeSpan.FromMinutes(1));
        }
    }
}
