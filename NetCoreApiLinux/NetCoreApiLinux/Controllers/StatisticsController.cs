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
        private readonly IAppInfoRepository appInfoRepository;
        private static readonly ILogger log = Log.ForContext<StatisticsController>();

        public StatisticsController(IAppInfoRepository appInfoRepository)
        {
            this.appInfoRepository = appInfoRepository;
        }

        [HttpPost("appInfo")]
        public void AddOrUpdateAppInfo([FromBody] AppInfoRequest request)
        {
            if (request.AppInfo.Id == null)
                throw new ArgumentException("Id should not be null.");

            appInfoRepository.AddOrUpdateAppInfo(request.AppInfo.Adapt<AppInfoDbo>());

            log.Information("Called {Method}, {@Request}", nameof(AddOrUpdateAppInfo), request);
        }

        [HttpGet("appInfo/all")]
        public AppInfo[] GetAllAppInfos()
        {
            log.Information($"Called {nameof(GetAllAppInfos)}");
            return appInfoRepository.GetAll().Select(x => x.Adapt<AppInfo>()).ToArray();
        }
    }
}
