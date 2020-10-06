using System;

namespace DataLayer.Dbo.AppInfo
{
    public class AppIdDbo : IMongoBsonId
    {
        public Guid DeviceId { get; set; }

        public string ToBsonId() => DeviceId.ToString();
    }
}
