using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiLinux.Models.StatInfo;
using Serilog;

namespace NetCoreApiLinux.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatisticsController : ControllerBase
    {
        private static readonly IList<StatInfo> stats = new List<StatInfo>();

        [HttpPost]
        [Route("addStatInfo")]
        public void AddOrUpdateStatInfo([FromBody] StatInfoClientId clientId, string appVersion)
        {
            var stat = stats.SingleOrDefault(x => Equals(clientId, x.ClientId));

            if (stat == null)
            {
                AddStatInfo(clientId, appVersion);
                return;
            }

            stat.Data.AppVersion = appVersion;
            stat.LastUpdatedAt = DateTime.Now;

            Log.Information($"Called {nameof(AddOrUpdateStatInfo)}");
        }

        [HttpGet]
        [Route("allStats")]
        public IEnumerable<StatInfo> GetAllStats()
        {
            Log.Information($"Called {nameof(GetAllStats)}");
            return stats;
        }

        private static void AddStatInfo(StatInfoClientId clientId, string appVersion)
        {
            var statInfo = new StatInfo
            {
                ClientId = clientId,
                Data = new StatInfoData
                {
                    AppVersion = appVersion
                },
                LastUpdatedAt = DateTime.Now
            };

            stats.Add(statInfo);
            Log.Information($"Called {nameof(AddStatInfo)}");
        }

        private static bool Equals(StatInfoClientId l, StatInfoClientId r) =>
            l.Id == r.Id && l.OsName == r.OsName && l.UserName == r.UserName;
    }
}
