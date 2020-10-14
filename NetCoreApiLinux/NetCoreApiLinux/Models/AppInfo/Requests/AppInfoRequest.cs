namespace NetCoreApiLinux.Models.AppInfo.Requests
{
    public class AppInfoRequest
    {
        public AppInfo AppInfo { get; set; }
        public StatisticsEvent[] Events { get; set; }
    }
}
