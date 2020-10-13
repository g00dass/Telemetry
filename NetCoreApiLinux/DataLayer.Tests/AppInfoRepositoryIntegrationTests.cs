using System.Threading.Tasks;
using AutoFixture.Xunit2;
using DataLayer.Dbo.AppInfo;
using FluentAssertions;
using Xunit;

namespace DataLayer.Tests
{
    // before test run IntegrationTests/ docker compose up --build
    public class AppInfoRepositoryIntegrationTests 
    {
        private readonly IAppInfoRepository repository;

        public AppInfoRepositoryIntegrationTests(IAppInfoRepository repository)
        {
            this.repository = repository;
        }

        [Theory, AutoData]
        public async Task Test(AppInfoDbo dbo1, AppInfoDbo dbo2)
        {
            await repository.AddOrUpdateAsync(dbo1);
            await repository.AddOrUpdateAsync(dbo2);
            (await repository.GetAllAsync()).Should().HaveCount(2);

            await repository.DeleteByIdAsync(dbo1.Id);
            await repository.DeleteByIdAsync(dbo2.Id);
            (await repository.GetAllAsync()).Should().HaveCount(0);
        }
    }
}
