using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Dbo.AppInfo;
using Mapster;

namespace DataLayer
{
    public interface IAppInfoRepository
    {
        AppInfoDbo[] GetAll();
        AppInfoDbo Find(AppIdDbo appId);
        void AddOrUpdateAppInfo(AppInfoDbo dbo);
    }

    public class AppInfoRepository : IAppInfoRepository
    {
        private static readonly IList<AppInfoDbo> appInfos = new List<AppInfoDbo>();

        public AppInfoDbo[] GetAll()
        {
            return appInfos.ToArray();
        }

        public AppInfoDbo Find(AppIdDbo appId)
        {
            return appInfos.SingleOrDefault(x => x.Id.DeviceId == appId.DeviceId);
        }

        public void AddOrUpdateAppInfo(AppInfoDbo dbo)
        {
            var oldDbo = Find(dbo.Id);
            if(oldDbo == null)
                appInfos.Add(CreateAppInfo(dbo));
            else
                UpdateAppInfo(oldDbo, dbo);
        }

        private static AppInfoDbo CreateAppInfo(AppInfoDbo dbo)
        {
            dbo.LastUpdatedAt = DateTimeOffset.Now;

            return dbo;
        }

        private static AppInfoDbo UpdateAppInfo(AppInfoDbo oldDbo, AppInfoDbo newDbo)
        {
            oldDbo.Statistics = newDbo.Statistics.Adapt<AppStatisticsDbo>();
            oldDbo.LastUpdatedAt = DateTimeOffset.Now;

            return oldDbo;
        }
    }
}
