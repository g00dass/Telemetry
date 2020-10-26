using System;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Dbo;
using DataLayer.Kafka;
using DataLayer.Models.AppInfo;
using DataLayer.Providers;
using DataLayer.Repository;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiLinux.Models.AppInfo;
using NetCoreApiLinux.Models.AppInfo.Requests;
using NetCoreApiLinux.Models.AppInfo.Responses;
using Serilog;

namespace NetCoreApiLinux.Controllers
{
    [ApiController]
    [Route("statistics/api")]
    public class StatisticsController : ControllerBase
    {
        private static readonly ILogger log = Log.ForContext<StatisticsController>();
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly ICriticalEventsProducer criticalEventsProducer;
        private readonly IStatisticsEventTypeProvider statisticsEventTypeProvider;
        private readonly IStatisticsEventTypeSaver statisticsEventTypeSaver;
        private readonly IStatisticsEventProvider statisticsEventProvider;

        public StatisticsController(IUnitOfWorkFactory unitOfWorkFactory,
            ICriticalEventsProducer criticalEventsProducer,
            IStatisticsEventTypeProvider statisticsEventTypeProvider,
            IStatisticsEventTypeSaver statisticsEventTypeSaver,
            IStatisticsEventProvider statisticsEventProvider)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.criticalEventsProducer = criticalEventsProducer;
            this.statisticsEventTypeProvider = statisticsEventTypeProvider;
            this.statisticsEventTypeSaver = statisticsEventTypeSaver;
            this.statisticsEventProvider = statisticsEventProvider;
        }

        /// <summary>
        /// Create or update statistics meta and merge events history
        /// </summary>
        /// <response code="200">AppInfoResponseDto created or updated</response>
        [HttpPost("appInfo")]
        public async Task AddOrUpdateAppInfoAsync([FromBody] AppInfoCreateRequest createRequest)
        {
            if (createRequest.AppInfo.Id == null)
                throw new ArgumentException("Id should not be null.");

            using (var uof = unitOfWorkFactory.Create())
            {
                var events = createRequest.Events.Select(x => x.Adapt<StatisticsEventDbo>());
                await uof.GetRepository<IStatisticsEventRepository>().AddAsync(events.ToArray(), createRequest.AppInfo.Id.ToString()).ConfigureAwait(false);

                var newAppInfo = createRequest.AppInfo.Adapt<AppInfoDbo>();
                await uof.GetRepository<IAppInfoRepository>().AddOrUpdateAsync(newAppInfo).ConfigureAwait(false);

                await uof.SaveChanges();
            }

            var criticalEvents = await statisticsEventProvider.
                EnrichDescription(
                    createRequest
                        .Events
                        .Where(x => x.IsCritical == true)
                        .Select(x => x.Adapt<StatisticsEvent>()));

            criticalEventsProducer.Send(createRequest.AppInfo.Id, criticalEvents);

            log.Information("Called {Method}, {@Request}", nameof(AddOrUpdateAppInfoAsync), createRequest);
        }

        /// <summary>
        /// Get statistics meta for all devices
        /// </summary>
        /// <response code="200">Array with AppInfos for all devices</response>
        [HttpGet("appInfo/all")]
        [ProducesResponseType(typeof(AppInfoResponseDto[]), 200)]
        public async Task<AppInfoResponseDto[]> GetAllAppInfosAsync()
        {
            log.Information($"Called {nameof(GetAllAppInfosAsync)}");

            using var uof = unitOfWorkFactory.Create();

            return (await uof.GetRepository<IAppInfoRepository>()
                    .GetAllAsync()
                    .ConfigureAwait(false))
                .Select(x => x.Adapt<AppInfoResponseDto>())
                .ToArray();
        }

        /// <summary>
        /// Get statistics meta for device by id
        /// </summary>
        /// <response code="200">AppInfoResponseDto for device with provided id</response>
        [HttpGet("appInfo/{id}")]
        [ProducesResponseType(typeof(AppInfoResponseDto), 200)]
        public async Task<AppInfoResponseDto> GetAppInfoByIdAsync(Guid id)
        {
            log.Information($"Called {nameof(GetAppInfoByIdAsync)}");

            using var uof = unitOfWorkFactory.Create();

            return (await uof.GetRepository<IAppInfoRepository>()
                    .FindAsync(id.ToString())
                    .ConfigureAwait(false))
                .Adapt<AppInfoResponseDto>();
        }

        /// <summary>
        /// Get statistics events history for device by id
        /// </summary>
        /// <response code="200">Array with StatisticsEvent for device with provided id</response>
        [HttpGet("appInfo/{id}/events-history")]
        [ProducesResponseType(typeof(StatisticsEventResponseDto[]), 200)]
        public async Task<StatisticsEventResponseDto[]> GetStatisticsEventsHistoryByAppInfoIdAsync(Guid id)
        {
            log.Information($"Called {nameof(GetStatisticsEventsHistoryByAppInfoIdAsync)}");

            using var uof = unitOfWorkFactory.Create();

            return (await statisticsEventProvider.GetByDeviceIdAsync(id)).Select(x => x.Adapt<StatisticsEventResponseDto>()).ToArray();
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

            using (var uof = unitOfWorkFactory.Create())
            {
                await uof.GetRepository<IStatisticsEventRepository>()
                    .DeleteByDeviceIdAsync(id.ToString())
                    .ConfigureAwait(false);

                await uof.SaveChanges();
            }
        }

        /// <summary>
        /// Get StatisticsEventTypes for all types
        /// </summary>
        /// <response code="200">Array with StatisticsEventType for all types</response>
        [HttpGet("event-types")]
        [ProducesResponseType(typeof(StatisticsEventTypeDto[]), 200)]
        public async Task<StatisticsEventTypeDto[]> GetStatisticsEventsTypesAsync()
        {
            log.Information($"Called {nameof(GetStatisticsEventsTypesAsync)}");

            return (await statisticsEventTypeProvider.GetAllAsync().ConfigureAwait(false))
                .Values
                .Select(x => x.Adapt<StatisticsEventTypeDto>())
                .ToArray();
        }

        /// <summary>
        /// Create or update StatisticsEventTypes
        /// </summary>
        /// <response code="200">StatisticsEventTypes created or updated</response>
        [HttpPost("event-types")]
        public async Task CreateOrUpdateStatisticsEventsTypesAsync([FromBody] StatisticsEventTypeDto[] types)
        {
            log.Information($"Called {nameof(GetStatisticsEventsTypesAsync)}");

            if (types.Any(x => x.Description.Length > 50))
                throw new ArgumentException("Description length should be <= 50.");

            await statisticsEventTypeSaver
                .AddOrUpdateAsync(types.Select(x => x.Adapt<StatisticsEventTypeDbo>()).ToArray())
                .ConfigureAwait(false);
        }
    }
}
