using System.Threading.Tasks;
using DataLayer.Dbo.AppInfo;
using MongoDB.Driver;
using MoreLinq;

namespace DataLayer
{
    public interface IStatisticsEventRepository
    {
        Task<StatisticsEventDbo[]> GetAllAsync();
        Task<StatisticsEventDbo[]> FindByDeviceIdAsync(string deviceId);
        Task AddAsync(StatisticsEventDbo[] dbos, string deviceId);
        Task AddAsync(IClientSessionHandle session, StatisticsEventDbo[] dbos, string deviceId);
        Task DeleteByIdAsync(string id);
    }

    public class StatisticsEventRepository : IStatisticsEventRepository
    {
        private readonly IMongoDatabase db;

        public StatisticsEventRepository(IMongoDbProvider dbProvider)
        {
            this.db = dbProvider.Db;
        }

        public async Task<StatisticsEventDbo[]> GetAllAsync()
        {
            return (await Events
                    .FindAsync(_ => true)
                    .ConfigureAwait(false))
                .ToList()
                .ToArray();
        }

        public async Task<StatisticsEventDbo[]> FindByDeviceIdAsync(string deviceId)
        {
            var filterEq = Builders<StatisticsEventDbo>.Filter.Eq(x => x.DeviceId, deviceId);

            return (await Events
                    .FindAsync(filterEq)
                    .ConfigureAwait(false))
                .ToList()
                .ToArray();
        }

        public Task AddAsync(StatisticsEventDbo[] dbos, string deviceId)
        {
            dbos.ForEach(x => x.DeviceId = deviceId);

            return Events.InsertManyAsync(dbos);
        }

        public Task AddAsync(IClientSessionHandle session, StatisticsEventDbo[] dbos, string deviceId)
        {
            dbos.ForEach(x => x.DeviceId = deviceId);

            return Events.InsertManyAsync(session, dbos);
        }

        public Task DeleteByIdAsync(string id)
        {
            var filterEq = Builders<StatisticsEventDbo>.Filter.Eq(x => x.Id, id);

            return Events
                .DeleteOneAsync(filterEq);
        }

        private IMongoCollection<StatisticsEventDbo> Events => db.GetCollection<StatisticsEventDbo>("StatisticsEvents");
    }
}
