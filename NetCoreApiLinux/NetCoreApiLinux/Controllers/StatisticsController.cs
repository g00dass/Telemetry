using System;
using System.Linq;
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
        private readonly IStatisticsEventsHistoryRepository eventsRepository;

        public StatisticsController(IAppInfoRepository appInfoRepository, IStatisticsEventsHistoryRepository eventsRepository)
        {
            this.appInfoRepository = appInfoRepository;
            this.eventsRepository = eventsRepository;
        }

        /// <summary>
        /// Create or update statistics meta and merge events history
        /// </summary>
        /// <response code="200">AppInfo created</response>
        [HttpPost("appInfo")]
        public void AddOrUpdateAppInfo([FromBody] AppInfoRequest request)
        {
            if (request.AppInfo.Id == null)
                throw new ArgumentException("Id should not be null.");

            if (request.Events.Any(x => x.Description.Length > 50))
                throw new ArgumentException("Description length should be <= 50.");

            var events = request.Adapt<StatisticsEventsHistoryDbo>();
            events.Id = request.AppInfo.Id.ToString();
            var latestDate = eventsRepository.AddOrUpdate(events);

            var newAppInfo = request.AppInfo.Adapt<AppInfoDbo>();
            newAppInfo.LastUpdatedAt = latestDate ?? DateTimeOffset.Now;
            appInfoRepository.AddOrUpdate(newAppInfo);

            log.Information("Called {Method}, {@Request}", nameof(AddOrUpdateAppInfo), request);
        }

        /// <summary>
        /// Get statistics meta for all devices
        /// </summary>
        /// <response code="200">Array with AppInfos for all devices</response>
        [HttpGet("appInfo/all")]
        [ProducesResponseType(typeof(AppInfo[]), 200)]
        public AppInfo[] GetAllAppInfos()
        {
            log.Information($"Called {nameof(GetAllAppInfos)}");
            return appInfoRepository.GetAll().Select(x => x.Adapt<AppInfo>()).ToArray();
        }

        /// <summary>
        /// Get statistics meta for device by id
        /// </summary>
        /// <response code="200">AppInfo for device with provided id</response>
        [HttpGet("appInfo/{id}")]
        [ProducesResponseType(typeof(AppInfo), 200)]
        public AppInfo GetAppInfoById(Guid id)
        {
            log.Information($"Called {nameof(GetAppInfoById)}");
            return appInfoRepository.Find(id.ToString()).Adapt<AppInfo>();
        }

        /// <summary>
        /// Get statistics events history for device by id
        /// </summary>
        /// <response code="200">Array with StatisticsEvent for device with provided id</response>
        [HttpGet("appInfo/{id}/events-history")]
        [ProducesResponseType(typeof(StatisticsEvent[]), 200)]
        public StatisticsEvent[] GetStatisticsEventsHistoryByAppInfoId(Guid id)
        {
            log.Information($"Called {nameof(GetStatisticsEventsHistoryByAppInfoId)}");
            return eventsRepository.Find(id.ToString()).Events.Select(x => x.Adapt<StatisticsEvent>()).ToArray();
        }
    }
}
