using System;
using MongoDB.Bson.Serialization.Attributes;

namespace DataLayer.Dbo.AppInfo
{
    public class AppInfoDbo
    {
        [BsonId]
        public string Id { get; set; }

        public string AppVersion { get; set; }
        public string UserName { get; set; }
        public string OsName { get; set; }

        public DateTimeOffset LastUpdatedAt { get; set; }
    }
}
