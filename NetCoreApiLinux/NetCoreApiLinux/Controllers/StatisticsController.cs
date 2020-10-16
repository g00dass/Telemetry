using System;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Dbo.AppInfo;
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
        private readonly IAppInfoRepository appInfoRepository;
        private readonly IStatisticsEventRepository eventsRepository;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public StatisticsController(IAppInfoRepository appInfoRepository, IStatisticsEventRepository eventsRepository, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.appInfoRepository = appInfoRepository;
            this.eventsRepository = eventsRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Create or update statistics meta and merge events history
        /// </summary>
        /// <response code="200">AppInfo created</response>
        [HttpPost("appInfo")]
        public async Task AddOrUpdateAppInfoAsync([FromBody] AppInfoRequest request)
        {
            if (request.AppInfo.Id == null)
                throw new ArgumentException("Id should not be null.");

            if (request.Events.Any(x => x.Description.Length > 50))
                throw new ArgumentException("Description length should be <= 50.");

            await using (var uof = unitOfWorkFactory.Create())
            {
                uof.Session.StartTransaction();
                var events = request.Events.Select(x => x.Adapt<StatisticsEventDbo>());
                await eventsRepository.AddAsync(uof.Session, events.ToArray(), request.AppInfo.Id.ToString()).ConfigureAwait(false);

                var newAppInfo = request.AppInfo.Adapt<AppInfoDbo>();
                await appInfoRepository.AddOrUpdateAsync(uof.Session, newAppInfo).ConfigureAwait(false);
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
            return (await appInfoRepository
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
            return (await appInfoRepository
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
            return (await eventsRepository
                    .FindByDeviceIdAsync(id.ToString())
                    .ConfigureAwait(false))
                .Select(x => x.Adapt<StatisticsEvent>())
                .ToArray();
        }
    }
}
