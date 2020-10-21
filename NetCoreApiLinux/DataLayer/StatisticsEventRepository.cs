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
        Task DeleteByIdAsync(string id);
        Task DeleteByDeviceIdAsync(string deviceId);
    }

    public class StatisticsEventRepository : IStatisticsEventRepository
    {
        private readonly IClientSessionHandle session;
        private readonly IMongoDatabase db;

        public StatisticsEventRepository(IMongoDatabase db, IClientSessionHandle session)
        {
            this.session = session;
            this.db = db;
        }

        public async Task<StatisticsEventDbo[]> GetAllAsync()
        {
            return (await Events
                    .FindAsync(session,_ => true)
                    .ConfigureAwait(false))
                .ToList()
                .ToArray();
        }

        public async Task<StatisticsEventDbo[]> FindByDeviceIdAsync(string deviceId)
        {
            var filterEq = Builders<StatisticsEventDbo>.Filter.Eq(x => x.DeviceId, deviceId);

            return (await Events
                    .FindAsync(session, filterEq)
                    .ConfigureAwait(false))
                .ToList()
                .ToArray();
        }

        public Task AddAsync(StatisticsEventDbo[] dbos, string deviceId)
        {
            dbos.ForEach(x => x.DeviceId = deviceId);

            return Events.InsertManyAsync(session, dbos);
        }

        public Task DeleteByIdAsync(string id)
        {
            var filterEq = Builders<StatisticsEventDbo>.Filter.Eq(x => x.Id, id);

            return Events
                .DeleteOneAsync(session, filterEq);
        }

        public Task DeleteByDeviceIdAsync(string deviceId)
        {
            var filterEq = Builders<StatisticsEventDbo>.Filter.Eq(x => x.DeviceId, deviceId);

            return Events
                .DeleteManyAsync(session, filterEq);
        }

        private IMongoCollection<StatisticsEventDbo> Events => db.GetCollection<StatisticsEventDbo>("StatisticsEvents");
    }
}
