namespace DataLayer.Kafka
{
    public interface IKafkaSettings
    {
        string BootstrapServers { get; }
    }

    public class KafkaSettings : IKafkaSettings
    {
        public string BootstrapServers => "kafka:9092";
    }

    public static class KafkaTopics
    {
        public static string CriticalEvents_V_1 => "CriticalEvents.V.1";
    }
}
