using System;
using DataLayer.Dbo.AppInfo;
using MongoDB.Driver;

namespace DataLayer
{
    public class AppInfoRepository : IRepository<AppInfoDbo, AppIdDbo>
    {
        private readonly IMongoDatabase db;
        private IMongoCollection<AppInfoDbo> AppInfos => db.GetCollection<AppInfoDbo>("AppInfos");

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

        public AppInfoDbo Find(AppIdDbo appId)
        {
            var filterEq = Builders<AppInfoDbo>.Filter.Eq(x => x.MongoId, appId.ToBsonId());

            return AppInfos
                .Find(filterEq)
                .SingleOrDefault();
        }

        public void AddOrUpdateAppInfo(AppInfoDbo dbo)
        {
            var filterEq = Builders<AppInfoDbo>.Filter.Eq(x => x.MongoId, dbo.Id.ToBsonId());
            dbo.LastUpdatedAt = DateTimeOffset.Now;

            AppInfos
                .FindOneAndReplace(filterEq, dbo, new FindOneAndReplaceOptions<AppInfoDbo> { IsUpsert = true });
        }
    }
}
