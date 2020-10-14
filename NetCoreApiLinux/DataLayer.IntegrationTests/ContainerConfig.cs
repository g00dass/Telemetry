using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration.Documents;
using Mongo.Migration.Startup;
using Mongo.Migration.Startup.DotNetCore;

namespace DataLayer.IntegrationTests
{
    public class ContainerConfig
    {
        public static IServiceCollection Configure(IServiceCollection services)
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
            services.AddSingleton<IMongoClientProvider, MongoClientProvider>();
            services.AddSingleton(x => x.GetRequiredService<IMongoClientProvider>().Client);

            var settings = new MongoMigrationSettings
            {
                ConnectionString = mongoSettings.ConnectionString,
                Database = mongoSettings.DatabaseName,
                DatabaseMigrationVersion = new DocumentVersion(1, 0, 0) // Optional
            };
            services.AddMigration(settings);

            return services;
        }
    }
}
