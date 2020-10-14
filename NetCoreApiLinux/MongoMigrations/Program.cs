using System;
using DataLayer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration.Documents;
using Mongo.Migration.Migrations.Adapters;
using Mongo.Migration.Startup;
using Mongo.Migration.Startup.Static;
using MongoDB.Driver;

namespace MongoMigrations
{
    public class Program
    {
        public static void Main()
        {
            IServiceCollection services = new ServiceCollection();
            var environmentName = Environment.GetEnvironmentVariable("Hosting:Environment");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();

            var mongoSettings = new MongoDbSettings();
            configuration.GetSection("MongoDbSettings").Bind(mongoSettings);
            services.AddSingleton<IMongoDbSettings>(mongoSettings);
            services.AddSingleton<IMongoClientProvider, MongoClientProvider>();
            services.AddSingleton(x => x.GetRequiredService<IMongoClientProvider>().Client);

            var serviceProvider = services.BuildServiceProvider();

            MongoMigrationClient.Initialize(
                serviceProvider.GetService<IMongoClient>(),
                new MongoMigrationSettings
                {
                    ConnectionString = mongoSettings.ConnectionString,
                    Database = mongoSettings.DatabaseName,
                    DatabaseMigrationVersion = new DocumentVersion(1, 0, 0) // Optional
                },
                new LightInjectAdapter(new LightInject.ServiceContainer()));

            Console.WriteLine("Apply database migrations");
        }
    }
}
