using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Dbo;
using DataLayer.Repository;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiLinux.Models.AppInfo;
using NetCoreApiLinux.Models.AppInfo.Requests;
using Serilog;

namespace NetCoreApiLinux.Controllers
{
    [ApiController]
    [Route("statistics/api")]
    public class StatisticsController : ControllerBase
    {
        private static readonly ILogger log = Log.ForContext<StatisticsController>();
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public StatisticsController(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Create or update statistics meta and merge events history
        /// </summary>
        /// <response code="200">AppInfo created or updated</response>
        [HttpPost("appInfo")]
        public async Task AddOrUpdateAppInfoAsync([FromBody] AppInfoRequest request)
        {
            if (request.AppInfo.Id == null)
                throw new ArgumentException("Id should not be null.");

            //if (request.Events.Any(x => x.Description.Length > 50))
            //    throw new ArgumentException("Description length should be <= 50.");

            await using (var uof = unitOfWorkFactory.Create())
            {
                var events = request.Events.Select(x => x.Adapt<StatisticsEventDbo>());
                await uof.GetRepository<IStatisticsEventRepository>().AddAsync(events.ToArray(), request.AppInfo.Id.ToString()).ConfigureAwait(false);

                var newAppInfo = request.AppInfo.Adapt<AppInfoDbo>();
                await uof.GetRepository<IAppInfoRepository>().AddOrUpdateAsync(newAppInfo).ConfigureAwait(false);
            }

            log.Information("Called {Method}, {@Request}", nameof(AddOrUpdateAppInfoAsync), request);
        }

        /// <summary>
        /// Get statistics meta for all devices
        /// </summary>
        /// <response code="200">Array with AppInfos for all devices</response>
        [HttpGet("appInfo/all")]
        [ProducesResponseType(typeof(AppInfo[]), 200)]
        public async Task<AppInfo[]> GetAllAppInfosAsync()
        {
            log.Information($"Called {nameof(GetAllAppInfosAsync)}");

            await using var uof = unitOfWorkFactory.Create();

            return (await uof.GetRepository<IAppInfoRepository>()
                    .GetAllAsync()
                    .ConfigureAwait(false))
                .Select(x => x.Adapt<AppInfo>())
                .ToArray();
        }

        /// <summary>
        /// Get statistics meta for device by id
        /// </summary>
        /// <response code="200">AppInfo for device with provided id</response>
        [HttpGet("appInfo/{id}")]
        [ProducesResponseType(typeof(AppInfo), 200)]
        public async Task<AppInfo> GetAppInfoByIdAsync(Guid id)
        {
            log.Information($"Called {nameof(GetAppInfoByIdAsync)}");

            await using var uof = unitOfWorkFactory.Create();

            return (await uof.GetRepository<IAppInfoRepository>()
                    .FindAsync(id.ToString())
                    .ConfigureAwait(false))
                .Adapt<AppInfo>();
        }

        /// <summary>
        /// Get statistics events history for device by id
        /// </summary>
        /// <response code="200">Array with StatisticsEvent for device with provided id</response>
        [HttpGet("appInfo/{id}/events-history")]
        [ProducesResponseType(typeof(StatisticsEvent[]), 200)]
        public async Task<StatisticsEvent[]> GetStatisticsEventsHistoryByAppInfoIdAsync(Guid id)
        {
            log.Information($"Called {nameof(GetStatisticsEventsHistoryByAppInfoIdAsync)}");

            await using var uof = unitOfWorkFactory.Create();

            var types = await uof.GetRepository<IStatisticsEventTypeRepository>().GetAllAsync().ConfigureAwait(false);

            var a = (await uof.GetRepository<IStatisticsEventRepository>()
                    .FindByDeviceIdAsync(id.ToString())
                    .ConfigureAwait(false));
                var b = a.Select(x => x.Adapt<StatisticsEvent>());
            return b
                .Do(x => x.Type.Description = types.GetValueOrDefault(x.Type.Name)?.Description)
                .ToArray();
        }

        /// <summary>
        ///Delete all statistics events history for device by id
        /// </summary>
        /// <response code="200">History deleted</response>
        [HttpDelete("appInfo/{id}/events-history")]
        [ProducesResponseType(typeof(void), 200)]
        public async Task DeleteAllStatisticsEventsHistoryByAppInfoIdAsync(Guid id)
        {
            log.Information($"Called {nameof(DeleteAllStatisticsEventsHistoryByAppInfoIdAsync)}");

            await using var uof = unitOfWorkFactory.Create();

            await uof.GetRepository<IStatisticsEventRepository>()
                .DeleteByDeviceIdAsync(id.ToString())
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get StatisticsEventTypes for all types
        /// </summary>
        /// <response code="200">Array with StatisticsEventType for all types</response>
        [HttpGet("event-types")]
        [ProducesResponseType(typeof(StatisticsEventType[]), 200)]
        public async Task<StatisticsEventType[]> GetStatisticsEventsTypesAsync()
        {
            log.Information($"Called {nameof(GetStatisticsEventsTypesAsync)}");

            await using var uof = unitOfWorkFactory.Create();

            return (await uof.GetRepository<IStatisticsEventTypeRepository>().GetAllAsync().ConfigureAwait(false))
                .Values
                .Select(x => x.Adapt<StatisticsEventType>())
                .ToArray();
        }

        /// <summary>
        /// Create or update StatisticsEventTypes
        /// </summary>
        /// <response code="200">StatisticsEventTypes created or updated</response>
        [HttpPost("event-types")]
        public async Task CreateOrUpdateStatisticsEventsTypesAsync([FromBody] StatisticsEventType[] types)
        {
            log.Information($"Called {nameof(GetStatisticsEventsTypesAsync)}");

            await using var uof = unitOfWorkFactory.Create();

            await uof.GetRepository<IStatisticsEventTypeRepository>()
                    .AddOrUpdateAsync(types.Select(x => x.Adapt<StatisticsEventTypeDbo>()).ToArray())
                    .ConfigureAwait(false);
        }
    }
}
