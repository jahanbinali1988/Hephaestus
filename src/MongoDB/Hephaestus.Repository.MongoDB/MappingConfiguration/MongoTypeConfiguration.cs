using Hephaestus.Repository.Abstraction.Base;
using MongoDB.Bson.Serialization;

namespace Hephaestus.Repository.MongoDB.MappingConfiguration
{
    public abstract class MongoTypeConfiguration<T> where T : Entity
    {
        public void Configure()
        {
            BsonClassMap.RegisterClassMap<T>(map =>
            {
                map.AutoMap();
            });
        }
    }
}
