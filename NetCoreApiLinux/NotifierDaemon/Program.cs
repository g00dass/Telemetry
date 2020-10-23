using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NotifierDaemon
{
    class Program
    {

        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            var environmentName = Environment.GetEnvironmentVariable("Hosting:Environment");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();

            var consumerSettings = new ConsumerSettings();

            configuration.GetSection("ConsumerSettings").Bind(consumerSettings);

            var serviceProvider = services.BuildServiceProvider();
            var runner = serviceProvider.GetService<IRunner>();
            runner.Run();
        }
    }
}
