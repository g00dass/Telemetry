using System;

namespace NetCoreApiLinux.Models.AppInfo.Requests
{
    public class AppInfoRequestDto
    {
        public Guid Id { get; set; }

        public string AppVersion { get; set; }
        public string UserName { get; set; }
        public string OsName { get; set; }
    }
}
