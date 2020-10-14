using System.Threading.Tasks;
using AutoFixture.Xunit2;
using DataLayer.Dbo.AppInfo;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration;
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
            //ServiceProvider.GetService<IMongoMigration>().Run();
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
            var all = await repository.GetAllAsync();

            await repository.DeleteByIdAsync(dbo1.Id);
            await repository.DeleteByIdAsync(dbo2.Id);

            all.Should().HaveCount(2);
            (await repository.GetAllAsync()).Should().HaveCount(0);
        }

        [Theory, AutoData]
        public async Task FindAsync_DbContainsOne_ShouldFindOne(AppInfoDbo dbo)
        {
            await repository.AddOrUpdateAsync(dbo);

            (await repository.FindAsync(dbo.Id)).Should().BeEquivalentTo(dbo);

            await repository.DeleteByIdAsync(dbo.Id);
        }
    }
}
