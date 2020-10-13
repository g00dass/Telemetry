using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var environmentName = Environment.GetEnvironmentVariable("Hosting:Environment");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();

            var mongoSettings = new MongoDbSettings();
            configuration.GetSection("MongoDbSettings").Bind(mongoSettings);
            services.AddSingleton<IMongoDbSettings>(mongoSettings);
            services.AddSingleton<IMongoDbProvider, MongoDbProvider>();
            services.AddSingleton<IAppInfoRepository, AppInfoRepository>();
            services.AddSingleton<IStatisticsEventRepository, StatisticsEventRepository>();
        }
    }
}
