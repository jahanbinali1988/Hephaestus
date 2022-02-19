using Hephaestus.Repository.MongoDB;
using SampleWebApiApplicationWithMongoDb.Models;
using System.Threading.Tasks;

namespace SampleWebApiApplicationWithMongoDb.Persistence
{
    public class PersonRepository : MongoDbBaseRepository<PersonEntity, long>, IPersonRepository
    {
        public PersonRepository(SampleMongoDbContext context) : base(context)
        {
        }

        public override Task<long> GetNextId()
        {
            throw new System.NotImplementedException();
        }
    }
}
