using Microsoft.Extensions.DependencyInjection;

namespace NotifierDaemon
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = ContainerConfig.Configure(new ServiceCollection());
            var serviceProvider = services.BuildServiceProvider();

            var consumer = serviceProvider.GetService<ICriticalEventsConsumer>();
            consumer.ProcessEvents();
        }
    }
}
