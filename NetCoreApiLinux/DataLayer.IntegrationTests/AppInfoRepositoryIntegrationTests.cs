using System.Threading.Tasks;
using AutoFixture.Xunit2;
using DataLayer.Dbo.AppInfo;
using FluentAssertions;
using Xunit;

namespace DataLayer.IntegrationTests
{
    // before test run docker compose up --build
    public class AppInfoRepositoryIntegrationTests 
    {
        private readonly IAppInfoRepository repository;

        public AppInfoRepositoryIntegrationTests(IAppInfoRepository repository)
        {
            this.repository = repository;
        }

        [Theory, AutoData]
        public async Task GetAllAsync_DbContainsSome_ShouldReturnAll(AppInfoDbo dbo1, AppInfoDbo dbo2)
        {
            await repository.AddOrUpdateAsync(dbo1);
            await repository.AddOrUpdateAsync(dbo2);
            (await repository.GetAllAsync()).Should().HaveCount(2);

            await repository.DeleteByIdAsync(dbo1.Id);
            await repository.DeleteByIdAsync(dbo2.Id);
            (await repository.GetAllAsync()).Should().HaveCount(0);
        }

        [Theory, AutoData]
        public async Task FindAsync_DbContainsOne_ShouldFindOne(AppInfoDbo dbo)
        {
            await repository.AddOrUpdateAsync(dbo);

            (await repository.FindAsync(dbo.Id)).LastUpdatedAt.Should().Be(dbo.LastUpdatedAt);

            await repository.DeleteByIdAsync(dbo.Id);
        }
    }
}
