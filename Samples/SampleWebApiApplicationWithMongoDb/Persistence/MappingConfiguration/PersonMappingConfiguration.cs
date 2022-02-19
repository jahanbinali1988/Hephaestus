using MongoDB.Bson.Serialization;
using SampleWebApiApplicationWithMongoDb.Models;

namespace SampleWebApiApplicationWithMongoDb.Persistence.MappingConfiguration
{
    public class PersonMappingConfiguration
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<PersonEntity>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.FirstName).SetIsRequired(true);
                map.MapMember(x => x.LastName).SetIsRequired(true);
            });
        }
    }
}
