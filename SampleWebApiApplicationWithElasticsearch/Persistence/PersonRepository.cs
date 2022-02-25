using Hephaestus.Repository.Elasticsearch;
using Nest;
using SampleWebApiApplicationWithElasticsearch.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleWebApiApplicationWithElasticsearch.Persistence
{
    public class PersonRepository : ElasticseachBaseRepository<PersonEntity, Guid>, IPersonRepository
    {
        public PersonRepository(SampleElasticDbContext dbContext) : base(dbContext)
        {
        }

    }
}
