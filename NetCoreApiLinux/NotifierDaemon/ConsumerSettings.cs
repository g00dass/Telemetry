namespace NotifierDaemon
{
    public interface IConsumerSettings
    {
        public string GroupId { get; set; }
    }

    public class ConsumerSettings : IConsumerSettings
    {
        public string GroupId { get; set; }
    }
}
