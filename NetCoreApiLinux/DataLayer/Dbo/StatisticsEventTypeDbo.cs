using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace DataLayer.Dbo
{
    public class StatisticsEventTypeDbo
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
