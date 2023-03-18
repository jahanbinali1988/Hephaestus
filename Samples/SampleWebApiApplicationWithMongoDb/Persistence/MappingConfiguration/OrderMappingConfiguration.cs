using MongoDB.Bson.Serialization;
using SampleWebApiApplicationWithMongoDb.Models;

namespace SampleWebApiApplicationWithMongoDb.Persistence.MappingConfiguration
{
    public class OrderMappingConfiguration
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<OrderEntity>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
            });
        }
    }
}
