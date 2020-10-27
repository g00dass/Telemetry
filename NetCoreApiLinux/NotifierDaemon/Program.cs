using Microsoft.Extensions.DependencyInjection;

namespace NotifierDaemon
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection().Configure().BuildServiceProvider();

            var consumer = serviceProvider.GetService<ICriticalEventsConsumer>();
            consumer.ProcessEvents();
        }
    }
}
