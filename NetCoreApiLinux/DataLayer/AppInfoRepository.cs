using System;
using DataLayer.Dbo.AppInfo;
using MongoDB.Driver;

namespace DataLayer
{
    public interface IAppInfoRepository
    {
        public AppInfoDbo[] GetAll();
        public AppInfoDbo Find(string id);
        public void AddOrUpdate(AppInfoDbo dbo);
    }

    public class AppInfoRepository : IAppInfoRepository
    {
        private readonly IMongoDatabase db;

        public AppInfoRepository(IMongoDbProvider dbProvider)
        {
            this.db = dbProvider.Db;
        }

        public AppInfoDbo[] GetAll()
        {
            return AppInfos
                .Find(_ => true)
                .ToList()
                .ToArray();
        }

        public AppInfoDbo Find(string id)
        {
            var filterEq = Builders<AppInfoDbo>.Filter.Eq(x => x.Id, id);

            return AppInfos
                .Find(filterEq)
                .SingleOrDefault();
        }

        public void AddOrUpdate(AppInfoDbo dbo)
        {
            var filterEq = Builders<AppInfoDbo>.Filter.Eq(x => x.Id, dbo.Id);

            AppInfos
                .FindOneAndReplace(filterEq, dbo, new FindOneAndReplaceOptions<AppInfoDbo> { IsUpsert = true });
        }

        private IMongoCollection<AppInfoDbo> AppInfos => db.GetCollection<AppInfoDbo>("AppInfos");
    }
}
