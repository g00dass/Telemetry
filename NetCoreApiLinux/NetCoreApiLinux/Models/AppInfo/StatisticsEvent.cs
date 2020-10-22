using System;

namespace NetCoreApiLinux.Models.AppInfo
{
    public class StatisticsEvent
    {
        public DateTimeOffset Date { get; set; }
        public StatisticsEventType Type { get; set; }
    }
}
