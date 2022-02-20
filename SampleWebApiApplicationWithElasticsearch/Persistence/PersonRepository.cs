using Hephaestus.Repository.Elasticsearch;
using SampleWebApiApplicationWithElasticsearch.Models;
using System;

namespace SampleWebApiApplicationWithElasticsearch.Persistence
{
    public class PersonRepository : ElasticseachBaseRepository<PersonEntity, Guid>, IPersonRepository
    {
        public PersonRepository(SampleElasticDbContext dbContext) : base(dbContext)
        {
        }
    }
}
