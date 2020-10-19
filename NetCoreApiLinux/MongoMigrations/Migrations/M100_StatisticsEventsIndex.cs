using DataLayer.Dbo.AppInfo;
using Mongo.Migration.Migrations.Database;
using MongoDB.Driver;

namespace MongoMigrations.Migrations
{
    public class M100_StatisticsEventsIndex : DatabaseMigration
    {
        public M100_StatisticsEventsIndex() : base("1.0.0")
        {
        }

        public override void Up(IMongoDatabase db)
        {
            db.CreateCollectionAsync("StatisticsEvents")
                .GetAwaiter().GetResult();

            var events = db.GetCollection<StatisticsEventDbo>("StatisticsEvents");

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
            db.DropCollectionAsync("StatisticsEvents").GetAwaiter().GetResult();
        }
    }
}
