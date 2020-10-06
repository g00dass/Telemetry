using System;
using DataLayer.Dbo.AppInfo;
using MongoDB.Driver;

namespace DataLayer
{
    public class AppInfoRepository : IRepository<AppInfoDbo, AppIdDbo>
    {
        private readonly IMongoDatabase db;

        public AppInfoRepository(IMongoDbSettings mongoDbSettings)
        {
            db = new MongoClient(mongoDbSettings.ConnectionString).GetDatabase(mongoDbSettings.DatabaseName);
        }

        public AppInfoDbo[] GetAll()
        {
            return AppInfos
                .Find(_ => true)
                .ToList()
                .ToArray();
        }

        public AppInfoDbo Find(AppIdDbo id)
        {
            var filterEq = Builders<AppInfoDbo>.Filter.Eq(x => x.MongoId, id.ToBsonId());

            return AppInfos
                .Find(filterEq)
                .SingleOrDefault();
        }

        public void AddOrUpdate(AppInfoDbo dbo)
        {
            var filterEq = Builders<AppInfoDbo>.Filter.Eq(x => x.MongoId, dbo.Id.ToBsonId());
            dbo.LastUpdatedAt = DateTimeOffset.Now;

            AppInfos
                .FindOneAndReplace(filterEq, dbo, new FindOneAndReplaceOptions<AppInfoDbo> { IsUpsert = true });
        }

        private IMongoCollection<AppInfoDbo> AppInfos => db.GetCollection<AppInfoDbo>("AppInfos");
    }
}
