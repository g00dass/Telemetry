using DataLayer.Kafka;
using Serilog;

namespace NotifierDaemon
{
    public interface IMailSender
    {
        void Send(CriticalEventMessage msg);
    }

    public class MailSender : IMailSender
    {
        private static readonly ILogger log = Log.ForContext<CriticalEventsConsumer>();

        public void Send(CriticalEventMessage msg)
        {
            Log.Information("Sent {@Message}", msg);
        }
    }
}
