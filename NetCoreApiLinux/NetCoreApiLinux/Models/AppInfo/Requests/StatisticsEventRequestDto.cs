using System;

namespace NetCoreApiLinux.Models.AppInfo.Requests
{
    public class StatisticsEventRequestDto
    {
        public DateTimeOffset Date { get; set; }
        public string TypeName { get; set; }
        public bool? IsCritical { get; set; }
    }
}
