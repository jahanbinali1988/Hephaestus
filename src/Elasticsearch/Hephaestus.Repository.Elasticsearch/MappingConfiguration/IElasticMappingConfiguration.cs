using Hephaestus.Repository.Abstraction.Base;
using Nest;

namespace Hephaestus.Repository.Elasticsearch.MappingConfiguration
{
    public interface IElasticMappingConfiguration<TEntity> where TEntity : Entity
    {
        string IdPropertyName { get; }

        public byte NumberOfShards { get; }

        public byte NumberOfReplicas { get; }

        string IndexName { get; }

        PropertiesDescriptor<TEntity> MapPropertiesDescriptor(PropertiesDescriptor<TEntity> descriptor);
    }
}
