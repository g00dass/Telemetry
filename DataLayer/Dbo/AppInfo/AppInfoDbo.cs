using System;

namespace NetCoreApiLinux.Models.AppInfo.Dbo
{
    public class AppInfoDbo
    {
        public AppIdDbo Id { get; set; }
        public AppStatisticsDbo Statistics { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
    }
}
