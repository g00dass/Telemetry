using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using DataLayer.Dbo.AppInfo;
using FluentAssertions;
using Xunit;

namespace DataLayer.IntegrationTests
{
    // before test run docker compose up --build
    public class StatisticsEventRepositoryIntegrationTests 
    {
        private readonly IStatisticsEventRepository repository;

        public StatisticsEventRepositoryIntegrationTests(IStatisticsEventRepository repository)
        {
            this.repository = repository;
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
