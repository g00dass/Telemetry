using System;
using MongoDB.Bson.Serialization.Attributes;

namespace DataLayer.Dbo.AppInfo
{
    public class AppInfoDbo
    {
        [BsonId]
        public string MongoId => Id.ToBsonId();

        public AppIdDbo Id { get; set; }
        public AppStatisticsDbo Statistics { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
    }
}
