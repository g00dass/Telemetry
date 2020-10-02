using System;
using System.Collections.Generic;
using System.Linq;
using Mapster;
using NetCoreApiLinux.Models.AppInfo.Requests;

namespace NetCoreApiLinux.Models.AppInfo
{
    public interface IAppInfoRepository
    {
        AppInfo[] GetAll();
        AppInfo Find(AppId appId);
        void AddOrUpdateAppInfo(AppInfoRequest request);
    }

    public class AppInfoRepository : IAppInfoRepository
    {
        private static readonly IList<AppInfo> appInfos = new List<AppInfo>();

        public AppInfo[] GetAll()
        {
            return appInfos.ToArray();
        }

        public AppInfo Find(AppId appId)
        {
            return appInfos.SingleOrDefault(x => x.Id.DeviceId == appId.DeviceId);
        }

        public void AddOrUpdateAppInfo(AppInfoRequest request)
        {
            var stat = Find(request.Id);
            if(stat == null)
                appInfos.Add(CreateAppInfo(request));
            else
                UpdateAppInfo(stat, request);
        }

        private static AppInfo CreateAppInfo(AppInfoRequest request)
        {
            var appInfo = request.Adapt<AppInfo>();
            appInfo.LastUpdatedAt = DateTimeOffset.Now;

            return appInfo;
        }

        private static AppInfo UpdateAppInfo(AppInfo appInfo, AppInfoRequest request)
        {
            appInfo.Statistics = request.Statistics.Adapt<AppStatistics>();
            appInfo.LastUpdatedAt = DateTimeOffset.Now;

            return appInfo;
        }
    }
}
