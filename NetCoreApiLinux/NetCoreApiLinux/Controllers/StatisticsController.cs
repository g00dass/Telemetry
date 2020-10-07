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
        private readonly IRepository<AppInfoDbo> appInfoRepository;

        public StatisticsController(IRepository<AppInfoDbo> appInfoRepository)
        {
            this.appInfoRepository = appInfoRepository;
        }

        /// <summary>
        /// Create or update statistics
        /// </summary>
        /// <response code="200">AppInfo created</response>
        [HttpPost("appInfo")]
        public void AddOrUpdateAppInfo([FromBody] AppInfoRequest request)
        {
            if (request.AppInfo.Id == null)
                throw new ArgumentException("Id should not be null.");

            appInfoRepository.AddOrUpdate(request.AppInfo.Adapt<AppInfoDbo>());

            log.Information("Called {Method}, {@Request}", nameof(AddOrUpdateAppInfo), request);
        }

        /// <summary>
        /// Get statistics for all devices
        /// </summary>
        /// <response code="200">Array with AppInfos for all devices</response>
        [HttpGet("appInfo/all")]
        [ProducesResponseType(typeof(AppInfo[]), 200)]
        public AppInfo[] GetAllAppInfos()
        {
            log.Information($"Called {nameof(GetAllAppInfos)}");
            return appInfoRepository.GetAll().Select(x => x.Adapt<AppInfo>()).ToArray();
        }
    }
}
