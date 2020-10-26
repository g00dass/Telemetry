using System;
using DataLayer.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NotifierDaemon
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

            var consumerSettings = new ConsumerSettings();
            configuration.GetSection("ConsumerSettings").Bind(consumerSettings);
            services.AddSingleton<IConsumerSettings>(consumerSettings);

            services.AddSingleton<ICriticalEventsConsumer, CriticalEventsConsumer>();
            services.AddSingleton<IMailSender, MailSender>();
            services.AddSingleton<IKafkaSettings, KafkaSettings>();

            return services;
        }
    }
}
