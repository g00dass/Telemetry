using System;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Dbo;
using DataLayer.Models.AppInfo;
using DataLayer.Repository;
using Mapster;
using Microsoft.Extensions.Caching.Memory;

namespace DataLayer.Providers
{
    public interface IStatisticsEventTypeSaver
    {
        Task AddOrUpdateAsync(StatisticsEventTypeDbo[] dbos);
    }

    public class StatisticsEventTypeSaver : IStatisticsEventTypeSaver
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IMemoryCache cache;

        public StatisticsEventTypeSaver(IUnitOfWorkFactory unitOfWorkFactory, IMemoryCache cache)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.cache = cache;
        }

        public async Task AddOrUpdateAsync(StatisticsEventTypeDbo[] dbos)
        {
            using (var uof = unitOfWorkFactory.Create())
            {
                await uof.GetRepository<IStatisticsEventTypeRepository>().AddOrUpdateAsync(dbos);
                await uof.SaveChanges();
            }

            UpdateCacheAsync();
        }

        private async Task UpdateCacheAsync()
        {
            using var uof = unitOfWorkFactory.Create();
            var allTypes = (await uof.GetRepository<IStatisticsEventTypeRepository>().GetAllAsync().ConfigureAwait(false))
                .ToDictionary(x => x.Name, x => x.Adapt<StatisticsEventType>());

            using var entry = cache.CreateEntry("GetAllAsync").SetValue(allTypes).SetSlidingExpiration(TimeSpan.FromMinutes(1));
        }
    }
}

