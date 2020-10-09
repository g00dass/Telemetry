using System;
using DataLayer.Dbo.AppInfo;
using Mongo.Migration.Migrations.Database;
using MongoDB.Driver;

namespace NetCoreApiLinux.Migrations
{
    public class M100_StatisticsEventsIndex : DatabaseMigration
    {
        public M100_StatisticsEventsIndex() : base("1.0.0")
        {
        }

        public override void Up(IMongoDatabase db)
        {
            var appInfos = db.GetCollection<AppInfoDbo>("AppInfos");
            var id1 = Guid.NewGuid().ToString();
            var id2 = Guid.NewGuid().ToString();
            appInfos.InsertMany(new []
            {
                new AppInfoDbo
                {
                    Id = id1,
                    AppVersion = "1.0.0",
                    OsName = "OsX",
                    UserName = "Уася Лучший",
                    LastUpdatedAt = DateTimeOffset.Now
                },
                new AppInfoDbo
                {
                    Id = id2,
                    AppVersion = "1.0.0",
                    OsName = "Windows",
                    UserName = "Петя Таксебе",
                    LastUpdatedAt = DateTimeOffset.Now
                }
            });

            var events = db.GetCollection<StatisticsEventDbo>("StatisticsEvents");
            events.InsertMany(new []
            {
                new StatisticsEventDbo
                {
                    DeviceId = id1,
                    Date = DateTimeOffset.Now,
                    Description = "Описание",
                    Name = "Событие"
                },
                new StatisticsEventDbo
                {
                    DeviceId = id1,
                    Date = DateTimeOffset.Now,
                    Description = "Описание",
                    Name = "Событие"
                },
                new StatisticsEventDbo
                {
                    DeviceId = id2,
                    Date = DateTimeOffset.Now,
                    Description = "Описание",
                    Name = "Событие"
                },
                new StatisticsEventDbo
                {
                    DeviceId = id2,
                    Date = DateTimeOffset.Now,
                    Description = "Описание",
                    Name = "Событие"
                }
            });
            var deviceIdIndex = Builders<StatisticsEventDbo>.IndexKeys.Ascending(x => x.Name);
            var dateIndex = Builders<StatisticsEventDbo>.IndexKeys.Descending(x => x.Date);
            var combinedIndex = Builders<StatisticsEventDbo>.IndexKeys.Combine(new[] { deviceIdIndex, dateIndex });
            var indexModel = new CreateIndexModel<StatisticsEventDbo>(combinedIndex, new CreateIndexOptions());
            events.Indexes.CreateOne(indexModel);
        }

        public override void Down(IMongoDatabase db)
        {
            throw new System.NotImplementedException();
        }
    }
}
