using System;

namespace DataLayer.Dbo.AppInfo
{
    public class AppInfoDbo
    {
        public AppIdDbo Id { get; set; }
        public AppStatisticsDbo Statistics { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
    }
}
