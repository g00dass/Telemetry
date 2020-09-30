using System;

namespace NetCoreApiLinux.Models.AppInfo
{
    public class AppInfo
    {
        public AppId Id { get; set; }
        public AppStatistics Statistics { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
    }
}
