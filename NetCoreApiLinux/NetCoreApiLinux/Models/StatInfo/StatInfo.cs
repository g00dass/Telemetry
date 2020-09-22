using System;

namespace NetCoreApiLinux.Models.StatInfo
{
    public class StatInfo
    {
        public StatInfoClientId ClientId { get; set; }
        public StatInfoData Data { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
