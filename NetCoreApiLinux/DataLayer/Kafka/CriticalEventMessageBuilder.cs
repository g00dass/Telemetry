using System;
using DataLayer.Models.AppInfo;

namespace DataLayer.Kafka
{
    public interface ICriticalEventMessageBuilder
    {
        CriticalEventMessage Build(Guid id, StatisticsEvent statisticsEvent);
    }

    public class CriticalEventMessageBuilder : ICriticalEventMessageBuilder
    {
        public CriticalEventMessage Build(Guid id, StatisticsEvent statisticsEvent) =>
            new CriticalEventMessage
            {
                Id = id,
                Date = statisticsEvent.Date,
                TypeName = statisticsEvent.Type.Name,
                TypeDescription = statisticsEvent.Type.Description
            };
    }
}
