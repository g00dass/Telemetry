using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace DataLayer.Dbo
{
    public class StatisticsEventDbo
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        public string DeviceId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string TypeName { get; set; }
        public bool? IsCritical { get; set; }

    }
}
