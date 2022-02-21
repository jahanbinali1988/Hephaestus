using Hephaestus.Repository.MongoDB;
using SampleWebApiApplicationWithMongoDb.Models;
using System;
using System.Threading.Tasks;

namespace SampleWebApiApplicationWithMongoDb.Persistence
{
    public class PersonRepository : MongoDbBaseRepository<PersonEntity, Guid>, IPersonRepository
    {
        public PersonRepository(SampleMongoDbContext context) : base(context)
        {
        }

        protected override string CollectionName => nameof(PersonEntity);

        public override Task<Guid> GetNextId()
        {
            return Task.FromResult<Guid>(Guid.NewGuid());
        }
    }
}
