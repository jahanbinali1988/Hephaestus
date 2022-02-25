using Hephaestus.Repository.Elasticsearch;
using Hephaestus.Repository.Elasticsearch.Configure;
using Microsoft.Extensions.Options;
using Hephaestus.Repository.Elasticsearch.Extensions;
using SampleWebApiApplicationWithElasticsearch.Models;
using SampleWebApiApplicationWithElasticsearch.Persistence.MappingConfiguration;
using Microsoft.Extensions.Logging;

namespace SampleWebApiApplicationWithElasticsearch.Persistence
{
    public class SampleElasticDbContext : ElasticDbContext
    {
        public SampleElasticDbContext(IOptions<ElasticsearchConfig> option) : base(option)
        {
        }

        protected override void OnModelCreating()
        {
            base._elasticClient.ApplyMappingConfiguration<PersonEntity>(base._connectionSettings, new PersonMappingConfiguration());

            base.OnModelCreating();
        }
    }
}
