using System;

namespace DataLayer.Kafka
{
    public class CriticalEventMessage
    {
        public Guid Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string TypeName { get; set; }
        public string TypeDescription { get; set; }
    }
}
