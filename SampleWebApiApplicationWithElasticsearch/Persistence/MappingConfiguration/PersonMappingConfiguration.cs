using Hephaestus.Repository.Elasticsearch.MappingConfiguration;
using Nest;
using SampleWebApiApplicationWithElasticsearch.Models;

namespace SampleWebApiApplicationWithElasticsearch.Persistence.MappingConfiguration
{
    public class PersonMappingConfiguration : IElasticMappingConfiguration<PersonEntity>
    {
        public string IdPropertyName => nameof(PersonEntity.Id);
        public byte NumberOfShards => 2;
        public byte NumberOfReplicas => 3;
        public string IndexName => "neptune-people-data";

        public PropertiesDescriptor<PersonEntity> MapPropertiesDescriptor(PropertiesDescriptor<PersonEntity> descriptor)
        {
            return descriptor
                    .Keyword(x => x.Name(n => n.FirstName))
                    .Keyword(x => x.Name(n => n.LastName))
                    .Date(x => x.Name(n => n.CreatedAt))
                    .Date(x => x.Name(n => n.ModifiedAt))
                    .Keyword(x => x.Name(n => n.Id).Index(true).Norms(false));
        }
    }
}
