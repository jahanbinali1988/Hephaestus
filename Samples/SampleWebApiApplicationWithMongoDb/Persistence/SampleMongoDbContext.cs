using Hephaestus.Repository.MongoDB;
using Hephaestus.Repository.MongoDB.Configure;
using Microsoft.Extensions.Options;
using SampleWebApiApplicationWithMongoDb.Persistence.MappingConfiguration;

namespace SampleWebApiApplicationWithMongoDb.Persistence
{
    public class SampleMongoDbContext : MongoContext
    {
        public SampleMongoDbContext(IOptions<MongoDbConfig> option) : base(option)
        {
        }

        protected override void OnModelCreating()
        {
            PersonMappingConfiguration.Configure();
            base.OnModelCreating();
        }
    }
}
