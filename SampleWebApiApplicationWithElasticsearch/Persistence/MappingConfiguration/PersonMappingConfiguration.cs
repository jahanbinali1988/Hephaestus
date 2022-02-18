using Hephaestus.Repository.Abstraction.Base;
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
                    .Number(x => x.Name(n => n.Id))
                    .Keyword(x => x.Name(n => n.FirstName))
                    .Keyword(x => x.Name(n => n.LastName))
                    .Date(x => x.Name(n => n.CreatedAt))
                    .Date(x => x.Name(n => n.ModifiedAt))
                    .Keyword(x => x.Name(n => n.UniqId).Index(true).Norms(false));
        }
    }
}
