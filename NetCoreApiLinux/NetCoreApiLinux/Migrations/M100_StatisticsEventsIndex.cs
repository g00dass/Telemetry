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
            var id1 = Guid.NewGuid().ToString();
            var id2 = Guid.NewGuid().ToString();

            var appInfos = db.GetCollection<AppInfoDbo>("AppInfos");
            appInfos.InsertManyAsync(new []
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
            }).GetAwaiter().GetResult();

            var events = db.GetCollection<StatisticsEventDbo>("StatisticsEvents");
            events.InsertManyAsync(new []
            {
                new StatisticsEventDbo
                {
                    DeviceId = id1,
                    Date = DateTimeOffset.Now,
                    Description = "Защита включена (установление защищенного соединения завершено)",
                    Name = "startVpnComplete"
                },
                new StatisticsEventDbo
                {
                    DeviceId = id1,
                    Date = DateTimeOffset.Now,
                    Description = "Режим экономии энергии выключен",
                    Name = "powerSaveModeOff"
                },
                new StatisticsEventDbo
                {
                    DeviceId = id2,
                    Date = DateTimeOffset.Now,
                    Description = "Установлено VPN-подключение в ОС",
                    Name = "vpnEstablished"
                },
                new StatisticsEventDbo
                {
                    DeviceId = id2,
                    Date = DateTimeOffset.Now,
                    Description = "Начало процесса включения защиты",
                    Name = "startVpn"
                }
            }).GetAwaiter().GetResult();

            var index = Builders<StatisticsEventDbo>
                .IndexKeys
                .Ascending(x => x.Name)
                .Descending(x => x.Date);

            events.Indexes
                .CreateOneAsync(new CreateIndexModel<StatisticsEventDbo>(index, new CreateIndexOptions()))
                .GetAwaiter().GetResult();
        }

        public override void Down(IMongoDatabase db)
        {
            db.DropCollectionAsync("AppInfos").GetAwaiter().GetResult();
            db.DropCollectionAsync("StatisticsEvents").GetAwaiter().GetResult();
        }
    }
}
