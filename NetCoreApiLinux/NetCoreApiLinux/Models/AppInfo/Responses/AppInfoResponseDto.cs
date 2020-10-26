using System;

namespace NetCoreApiLinux.Models.AppInfo.Responses
{
    public class AppInfoResponseDto
    {
        public Guid Id { get; set; }

        public string AppVersion { get; set; }
        public string UserName { get; set; }
        public string OsName { get; set; }

        public DateTimeOffset? LastUpdatedAt { get; set; }
    }
}
