using MongoDB.Bson.Serialization.Attributes;

namespace DataLayer.Dbo.AppInfo
{
    public class StatisticsEventsHistoryDbo
    {
        [BsonId]
        public string Id { get; set; }

        public StatisticsEventDbo[] Events { get; set; }
    }
}
