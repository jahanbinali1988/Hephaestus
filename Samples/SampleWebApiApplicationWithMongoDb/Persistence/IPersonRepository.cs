using Hephaestus.Repository.Abstraction.Contract;
using SampleWebApiApplicationWithMongoDb.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SampleWebApiApplicationWithMongoDb.Persistence
{
    public interface IPersonRepository : IRepository<PersonEntity, Guid>
    {
        Task<IEnumerable<PersonEntity>> GetAll(CancellationToken cancellationToken);
    }
}
