using System;

namespace NetCoreApiLinux.Models.AppInfo
{
    public class StatisticsEvent
    {
        public DateTimeOffset Date { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
