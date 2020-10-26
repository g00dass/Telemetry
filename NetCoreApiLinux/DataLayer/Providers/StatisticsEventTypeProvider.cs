using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models.AppInfo;
using DataLayer.Repository;
using Mapster;
using Microsoft.Extensions.Caching.Memory;

namespace DataLayer.Providers
{
    public interface IStatisticsEventTypeProvider
    {
        Task<Dictionary<string, StatisticsEventType>> GetAllAsync();
    }

    public class StatisticsEventTypeProvider : IStatisticsEventTypeProvider
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IMemoryCache cache;

        public StatisticsEventTypeProvider(IUnitOfWorkFactory unitOfWorkFactory, IMemoryCache cache)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.cache = cache;
        }

        public async Task<Dictionary<string, StatisticsEventType>> GetAllAsync()
        {
            return await cache.GetOrCreateAsync(
                nameof(GetAllAsync),
                async entry =>
                {
                    using (var uof = unitOfWorkFactory.Create())
                    {
                        entry.SlidingExpiration = TimeSpan.FromMinutes(1);

                        return (await uof.GetRepository<IStatisticsEventTypeRepository>().GetAllAsync().ConfigureAwait(false))
                            .ToDictionary(x => x.Name, x => x.Adapt<StatisticsEventType>());
                    }
                });
        }
    }
}
