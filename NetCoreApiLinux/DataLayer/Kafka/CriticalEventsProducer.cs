using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Confluent.Kafka;
using MoreLinq;
using Serilog;

namespace DataLayer.Kafka
{
    public interface ICriticalEventsProducer
    {
        void Send(IEnumerable<CriticalEventMessage> events);
    }

    public class CriticalEventsProducer : ICriticalEventsProducer
    {
        private static readonly ILogger log = Log.ForContext<CriticalEventsProducer>();
        private readonly IKafkaSettings kafkaSettings;

        public CriticalEventsProducer(IKafkaSettings kafkaSettings)
        {
            this.kafkaSettings = kafkaSettings;
        }

        public void Send(IEnumerable<CriticalEventMessage> events)
        {
            var config = new ProducerConfig { BootstrapServers = kafkaSettings.BootstrapServers };

            Action<DeliveryReport<Null, string>> handler = r =>
            {
                if (!r.Error.IsError)
                    log.Information($"Delivered message to {r.TopicPartitionOffset}");
                else
                    log.Error($"Delivery Error: {r.Error.Reason}");
            };

            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                foreach (var batch in events.Batch(100))
                {
                    batch.ForEach(x =>
                        p.Produce(
                            KafkaTopics.CriticalEvents_V_1,
                            new Message<Null, string>
                            {
                                Value = JsonSerializer.Serialize(x)
                            },
                            handler));

                    p.Flush(TimeSpan.FromSeconds(10));
                }
            }
        }
    }
}
