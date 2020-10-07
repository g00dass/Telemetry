using System;
using MongoDB.Bson.Serialization.Attributes;

namespace DataLayer.Dbo.AppInfo
{
    public class AppInfoDbo
    {
        [BsonId]
        public string Id { get; set; }

        public AppStatisticsDbo Statistics { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
    }
}
