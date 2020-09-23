using System;

namespace NetCoreApiLinux.Models.Application
{
    public class AppInfo
    {
        public AppId Id { get; set; }
        public AppStatistics Statistics { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
    }
}
