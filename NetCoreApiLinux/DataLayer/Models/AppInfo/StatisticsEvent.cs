using System;

namespace DataLayer.Models.AppInfo
{
    public class StatisticsEvent
    {
        public DateTimeOffset Date { get; set; }
        public StatisticsEventType Type { get; set; }
        public bool? IsCritical { get; set; }
    }
}