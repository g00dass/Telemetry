using System;

namespace NetCoreApiLinux.Models.AppInfo.Responses
{
    public class StatisticsEventResponseDto
    {
        public DateTimeOffset Date { get; set; }
        public StatisticsEventTypeDto Type { get; set; }
        public bool? IsCritical { get; set; }
    }
}
