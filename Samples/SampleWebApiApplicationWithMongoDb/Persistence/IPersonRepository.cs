using Hephaestus.Repository.Abstraction.Contract;
using SampleWebApiApplicationWithMongoDb.Models;

namespace SampleWebApiApplicationWithMongoDb.Persistence
{
    public interface IPersonRepository : IRepository<PersonEntity>
    {
    }
}
