using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace DataLayer.Dbo.AppInfo
{
    public class StatisticsEventDbo
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        public string DeviceId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
