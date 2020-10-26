namespace NetCoreApiLinux.Models.AppInfo.Requests
{
    public class AppInfoCreateRequest
    {
        public AppInfoRequestDto AppInfo { get; set; }
        public StatisticsEventRequestDto[] Events { get; set; }
    }
}
