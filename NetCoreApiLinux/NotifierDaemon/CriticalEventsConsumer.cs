using System;
using System.Text.Json;
using System.Threading;
using Confluent.Kafka;
using DataLayer.Kafka;
using Serilog;

namespace NotifierDaemon
{
    public interface ICriticalEventsConsumer
    {
        void ProcessEvents(CancellationToken token);
    }

    public class CriticalEventsConsumer : ICriticalEventsConsumer
    {
        private static readonly ILogger log = Log.ForContext<CriticalEventsConsumer>();
        private readonly IKafkaSettings kafkaSettings;
        private readonly IConsumerSettings consumerSettings;
        private readonly IMailSender mailSender;

        public CriticalEventsConsumer(
            IKafkaSettings kafkaSettings,
            IConsumerSettings consumerSettings,
            IMailSender mailSender)
        {
            this.kafkaSettings = kafkaSettings;
            this.consumerSettings = consumerSettings;
            this.mailSender = mailSender;
        }

        public void ProcessEvents(CancellationToken token)
        {
            var conf = new ConsumerConfig
            {
                GroupId = consumerSettings.GroupId,
                BootstrapServers = kafkaSettings.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            log.Information("Start consuming");

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe(KafkaTopics.CriticalEvents_V_1);

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(token);
                            var msg = JsonSerializer.Deserialize<CriticalEventMessage>(cr.Message.Value);
                            log.Information($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");

                            mailSender.Send(msg);
                        }
                        catch (ConsumeException e)
                        {
                            log.Error($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    c.Close();
                }
            }
        }
    }
}
