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
        Task<StatisticsEventTypeDbo[]> GetAllAsync();
        Task<StatisticsEventTypeDbo[]> FindByNameAsync(string name);
        Task AddOrUpdateAsync(StatisticsEventTypeDbo[] dbos);
    }

    public class StatisticsEventTypeRepository : IStatisticsEventTypeRepository
    {
        private readonly IClientSessionHandle session;
        private readonly IMongoDatabase db;

        public StatisticsEventTypeRepository(IMongoDatabase db, IClientSessionHandle session)
        {
            this.session = session;
            this.db = db;
        }

        public async Task<StatisticsEventTypeDbo[]> GetAllAsync()
        {
            return (await Types
                    .WithReadPreference(ReadPreference.SecondaryPreferred)
                    .FindAsync(session, _ => true)
                    .ConfigureAwait(false))
                .ToList()
                .ToArray();
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
        }

        private IMongoCollection<StatisticsEventTypeDbo> Types => db.GetCollection<StatisticsEventTypeDbo>("StatisticsEventTypes");
    }
}
