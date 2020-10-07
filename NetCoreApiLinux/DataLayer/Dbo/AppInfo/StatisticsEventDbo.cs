using System;

namespace DataLayer.Dbo.AppInfo
{
    public class StatisticsEventDbo
    {
        public DateTimeOffset Date { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
