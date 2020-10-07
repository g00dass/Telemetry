using System;
using System.Linq;
using DataLayer.Dbo.AppInfo;
using MongoDB.Driver;

namespace DataLayer
{
    public interface IStatisticsEventsHistoryRepository
    {
        StatisticsEventsHistoryDbo[] GetAll();
        StatisticsEventsHistoryDbo Find(string id);
        DateTimeOffset? AddOrUpdate(StatisticsEventsHistoryDbo dbo);
    }

    public class StatisticsEventsHistoryRepository : IStatisticsEventsHistoryRepository
    {
        private readonly IMongoDatabase db;

        public StatisticsEventsHistoryRepository(IMongoDbProvider dbProvider)
        {
            this.db = dbProvider.Db;
        }

        public StatisticsEventsHistoryDbo[] GetAll()
        {
            return Events
                .Find(_ => true)
                .ToList()
                .ToArray();
        }

        public StatisticsEventsHistoryDbo Find(string id)
        {
            var filterEq = Builders<StatisticsEventsHistoryDbo>.Filter.Eq(x => x.Id, id);

            return Events
                .Find(filterEq)
                .SingleOrDefault();
        }

        public DateTimeOffset? AddOrUpdate(StatisticsEventsHistoryDbo dbo)
        {
            var filterEq = Builders<StatisticsEventsHistoryDbo>.Filter.Eq(x => x.Id, dbo.Id);
            var update = Builders<StatisticsEventsHistoryDbo>.Update.PushEach(x => x.Events, dbo.Events);

            var newHistory = Events.FindOneAndUpdate(filterEq, update ,new FindOneAndUpdateOptions<StatisticsEventsHistoryDbo> { IsUpsert = true, ReturnDocument = ReturnDocument.After});
            return newHistory.Events.OrderByDescending(x => x.Date).FirstOrDefault()?.Date;
        }

        private IMongoCollection<StatisticsEventsHistoryDbo> Events => db.GetCollection<StatisticsEventsHistoryDbo>("StatisticsEventsHistories");
    }
}
