using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models.AppInfo;
using DataLayer.Repository;
using Mapster;
using NetCoreApiLinux;

namespace DataLayer.Providers
{
    public interface IStatisticsEventProvider
    {
        Task<StatisticsEvent[]> GetByDeviceIdAsync(Guid id);
        Task<StatisticsEvent[]> EnrichDescription(IEnumerable<StatisticsEvent> events);
    }

    public class StatisticsEventProvider : IStatisticsEventProvider
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IStatisticsEventTypeProvider statisticsEventTypeProvider;

        public StatisticsEventProvider(IUnitOfWorkFactory unitOfWorkFactory, IStatisticsEventTypeProvider statisticsEventTypeProvider)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.statisticsEventTypeProvider = statisticsEventTypeProvider;
        }

        public async Task<StatisticsEvent[]> GetByDeviceIdAsync(Guid id)
        {
            using var uof = unitOfWorkFactory.Create();

            var events = (await uof.GetRepository<IStatisticsEventRepository>()
                    .FindByDeviceIdAsync(id.ToString())
                    .ConfigureAwait(false))
                .Select(x => x.Adapt<StatisticsEvent>());

            return await EnrichDescription(events);
        }

        public async Task<StatisticsEvent[]> EnrichDescription(IEnumerable<StatisticsEvent> events)
        {
            var types = await statisticsEventTypeProvider.GetAllAsync();

            return events
                .Do(x => x.Type.Description = types.GetValueOrDefault(x.Type.Name)?.Description)
                .ToArray();
        }
    }
}
