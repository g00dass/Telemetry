using System;
using System.Linq;
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
        private readonly IAppInfoRepository appInfoRepository;
        private static readonly ILogger log =  Log.ForContext<StatisticsController>();

        public StatisticsController(IAppInfoRepository appInfoRepository)
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
            if (request.Id == null)
                throw new ArgumentException("Id should not be null.");

            appInfoRepository.AddOrUpdateAppInfo(request);

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
            return appInfoRepository.GetAll().ToArray();
        }
    }
}
