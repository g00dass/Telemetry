using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using DataLayer.Dbo.AppInfo;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration;
using Xunit;

namespace DataLayer.IntegrationTests
{
    public class StatisticsEventRepositoryDependencySetupFixture
    {
        public StatisticsEventRepositoryDependencySetupFixture()
        {
            IServiceCollection services = new ServiceCollection();

            services = ContainerConfig.Configure(services);

            services.AddSingleton<IStatisticsEventRepository, StatisticsEventRepository>();

            ServiceProvider = services.BuildServiceProvider();
            ServiceProvider.GetService<IMongoMigration>().Run();
        }

        public ServiceProvider ServiceProvider { get; private set; }
    }

    // before test run docker compose up --build
    public class StatisticsEventRepositoryIntegrationTests : IClassFixture<StatisticsEventRepositoryDependencySetupFixture>
    {
        private readonly IStatisticsEventRepository repository;

        public StatisticsEventRepositoryIntegrationTests(StatisticsEventRepositoryDependencySetupFixture fixture)
        {
            var serviceProvider = fixture.ServiceProvider;

            repository = serviceProvider.GetService<IStatisticsEventRepository>();
        }

        [Theory, AutoData]
        public async Task GetAllAsync_DbContainsSome_ShouldReturnAll(StatisticsEventDbo dbo1, StatisticsEventDbo dbo2, Guid deviceId)
        {
            await repository.AddAsync(new [] { dbo1, dbo2 }, deviceId.ToString());
            (await repository.GetAllAsync()).Should().HaveCount(2);
            (await repository.GetAllAsync()).First().DeviceId.Should().Be(deviceId.ToString());

            await repository.DeleteByIdAsync(dbo1.Id);
            await repository.DeleteByIdAsync(dbo2.Id);
            (await repository.GetAllAsync()).Should().HaveCount(0);
        }

        [Theory, AutoData]
        public async Task FindByDeviceIdAsync_DbContainsSome_ShouldFindThem(StatisticsEventDbo dbo1, StatisticsEventDbo dbo2, Guid deviceId)
        {
            await repository.AddAsync(new[] { dbo1, dbo2 }, deviceId.ToString());
            (await repository.FindByDeviceIdAsync(deviceId.ToString()))
                .Should()
                .HaveCount(2)
                .And.OnlyContain(x => x.DeviceId == deviceId.ToString());

            await repository.DeleteByIdAsync(dbo1.Id);
            await repository.DeleteByIdAsync(dbo2.Id);
        }
    }
}
