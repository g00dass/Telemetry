using System;

namespace DataLayer.Models.AppInfo
{
    public class AppInfo
    {
        public Guid Id { get; set; }

        public string AppVersion { get; set; }
        public string UserName { get; set; }
        public string OsName { get; set; }

        public DateTimeOffset? LastUpdatedAt { get; set; }
    }
}
