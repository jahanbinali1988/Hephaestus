using Hephaestus.Repository.Abstraction.Contract;
using SampleWebApiApplicationWithElasticsearch.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleWebApiApplicationWithElasticsearch.Persistence
{
    public interface IPersonRepository : IRepository<PersonEntity, Guid>
    {
    }
}
