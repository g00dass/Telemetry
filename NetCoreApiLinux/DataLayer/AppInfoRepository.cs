using System;
using System.Threading.Tasks;
using DataLayer.Dbo.AppInfo;
using MongoDB.Driver;

namespace DataLayer
{
    public interface IAppInfoRepository
    {
        public Task<AppInfoDbo[]> GetAllAsync();
        public Task<AppInfoDbo> FindAsync(string id);
        public Task AddOrUpdateAsync(AppInfoDbo dbo);
    }

    public class AppInfoRepository : IAppInfoRepository
    {
        private readonly IMongoDatabase db;

        public AppInfoRepository(IMongoDbProvider dbProvider)
        {
            this.db = dbProvider.Db;
        }

        public async Task<AppInfoDbo[]> GetAllAsync()
        {
            return (await AppInfos
                    .FindAsync(_ => true)
                    .ConfigureAwait(false))
                .ToList()
                .ToArray();
        }

        public async Task<AppInfoDbo> FindAsync(string id)
        {
            var filterEq = Builders<AppInfoDbo>.Filter.Eq(x => x.Id, id);

            return (await AppInfos
                    .FindAsync(filterEq)
                    .ConfigureAwait(false))
                .SingleOrDefault();
        }

        public async Task AddOrUpdateAsync(AppInfoDbo dbo)
        {
            var filterEq = Builders<AppInfoDbo>.Filter.Eq(x => x.Id, dbo.Id);

            dbo.LastUpdatedAt = DateTimeOffset.Now;;

            await AppInfos
                .FindOneAndReplaceAsync(filterEq, dbo, new FindOneAndReplaceOptions<AppInfoDbo> { IsUpsert = true })
                .ConfigureAwait(false);
        }

        private IMongoCollection<AppInfoDbo> AppInfos => db.GetCollection<AppInfoDbo>("AppInfos");
    }
}
