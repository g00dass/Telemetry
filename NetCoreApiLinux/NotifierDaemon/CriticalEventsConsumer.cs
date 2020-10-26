using System;
using System.Text.Json;
using System.Threading;
using Confluent.Kafka;
using DataLayer.Kafka;

namespace NotifierDaemon
{
    public interface ICriticalEventsConsumer
    {
        void ProcessEvents();
    }

    public class CriticalEventsConsumer : ICriticalEventsConsumer
    {
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

        public void ProcessEvents()
        {
            var conf = new ConsumerConfig
            {
                GroupId = consumerSettings.GroupId,
                BootstrapServers = kafkaSettings.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            Console.WriteLine("Start consuming");

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe(KafkaTopics.CriticalEvents_V_1);

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            var msg = JsonSerializer.Deserialize<CriticalEventMessage>(cr.Message.Value);
                            Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");

                            mailSender.Send(msg);
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
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
