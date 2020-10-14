using System.Threading.Tasks;
using AutoFixture.Xunit2;
using DataLayer.Dbo.AppInfo;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DataLayer.IntegrationTests
{
    public class AppInfoRepositoryDependencySetupFixture
    {
        public AppInfoRepositoryDependencySetupFixture()
        {
            IServiceCollection services = new ServiceCollection();

            services = ContainerConfig.Configure(services);
            services.AddSingleton<IAppInfoRepository, AppInfoRepository>();

            ServiceProvider = services.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; private set; }
    }

    // before test run docker compose up --build
    public class AppInfoRepositoryIntegrationTests : IClassFixture<AppInfoRepositoryDependencySetupFixture>
    {
        private readonly IAppInfoRepository repository;

        public AppInfoRepositoryIntegrationTests(AppInfoRepositoryDependencySetupFixture fixture)
        {
            var serviceProvider = fixture.ServiceProvider;
            repository = serviceProvider.GetService<IAppInfoRepository>();
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
