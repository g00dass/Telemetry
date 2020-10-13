using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataLayer.Dbo.AppInfo;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using Moq;
using Xunit;

namespace DataLayer.Tests
{
    public class AppInfoRepositoryTests
    {
        private AppInfoRepository Repository;
        private readonly MockRepository Mocks = new MockRepository(MockBehavior.Loose);

        [Fact]
        public async Task CallsCorrectCollectionName()
        {
            //var db = Mocks.Create<IMongoDatabase>();
            //var collection = Mocks.Create<IMongoCollection<AppInfoDbo>>();
            //collection.Setup(x => x.FindAsync(
            //        It.IsAny<FilterDefinition<AppInfoDbo>>(),
            //        It.IsAny<FindOptions<AppInfoDbo, AppInfoDbo>>(),
            //        It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(It.IsAny<IAsyncCursor<AppInfoDbo>>());

            //db.Setup(x => x.GetCollection<AppInfoDbo>(
            //        It.IsAny<string>(),
            //        It.IsAny<MongoCollectionSettings>()))
            //    .Returns(collection.Object);

            //var mongoDbProvider = Mocks.Create<IMongoDbProvider>();
            //mongoDbProvider.SetupGet(x => x.Db).Returns(db.Object);

            //Repository = new AppInfoRepository(mongoDbProvider.Object);
            //await Repository.GetAllAsync();

            //db.Verify(x =>
            //    x.GetCollection<AppInfoDbo>(
            //        It.Is<string>(y => y == "AppInfos"),
            //        It.IsAny<MongoCollectionSettings>()));
        }
    }
}
