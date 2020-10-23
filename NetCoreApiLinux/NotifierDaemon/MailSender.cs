using DataLayer.Kafka;

namespace NotifierDaemon
{
    public interface IMailSender
    {
        void Send(CriticalEventMessage msg);
    }

    public class MailSender : IMailSender
    {
        public void Send(CriticalEventMessage msg)
        {

        }
    }
}
