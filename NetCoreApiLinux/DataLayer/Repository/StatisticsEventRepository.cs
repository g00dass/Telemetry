using System.Threading.Tasks;
using DataLayer.Dbo;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using MoreLinq;

namespace DataLayer.Repository
{
    public interface IStatisticsEventRepository
    {
        Task<StatisticsEventDbo[]> GetAllAsync();
        Task<StatisticsEventDbo[]> FindByDeviceIdAsync(string deviceId);
        Task AddAsync(StatisticsEventDbo[] dbos, string deviceId);
        Task DeleteByIdAsync(string id);
        Task DeleteByDeviceIdAsync(string deviceId);
    }

    public class StatisticsEventRepository : IStatisticsEventRepository
    {
        private readonly IClientSessionHandle session;
        private readonly IMemoryCache cache;
        private readonly IMongoDatabase db;

        public StatisticsEventRepository(IMongoDatabase db, IClientSessionHandle session, IMemoryCache cache)
        {
            this.session = session;
            this.cache = cache;
            this.db = db;
        }

        public async Task<StatisticsEventDbo[]> GetAllAsync()
        {
            return (await Events
                    .WithReadPreference(ReadPreference.SecondaryPreferred)
                    .FindAsync(session,_ => true)
                    .ConfigureAwait(false))
                .ToList()
                .ToArray();
        }

        public async Task<StatisticsEventDbo[]> FindByDeviceIdAsync(string deviceId)
        {
            var filterEq = Builders<StatisticsEventDbo>.Filter.Eq(x => x.DeviceId, deviceId);

            return (await Events
                    .WithReadPreference(ReadPreference.SecondaryPreferred)
                    .FindAsync(session, filterEq)
                    .ConfigureAwait(false))
                .ToList()
                .ToArray();
        }

        public Task AddAsync(StatisticsEventDbo[] dbos, string deviceId)
        {
            dbos.ForEach(x => x.DeviceId = deviceId);

            return Events
                .WithWriteConcern(WriteConcern.WMajority)
                .InsertManyAsync(session, dbos);
        }

        public Task DeleteByIdAsync(string id)
        {
            var filterEq = Builders<StatisticsEventDbo>.Filter.Eq(x => x.Id, id);

            return Events
                .WithWriteConcern(WriteConcern.WMajority)
                .DeleteOneAsync(session, filterEq);
        }

        public Task DeleteByDeviceIdAsync(string deviceId)
        {
            var filterEq = Builders<StatisticsEventDbo>.Filter.Eq(x => x.DeviceId, deviceId);

            return Events
                .WithWriteConcern(WriteConcern.WMajority)
                .DeleteManyAsync(session, filterEq);
        }

        private IMongoCollection<StatisticsEventDbo> Events => db.GetCollection<StatisticsEventDbo>("StatisticsEvents");
    }
}
