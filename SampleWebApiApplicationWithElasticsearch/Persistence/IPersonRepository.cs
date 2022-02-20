using Hephaestus.Repository.Abstraction.Contract;
using SampleWebApiApplicationWithElasticsearch.Models;
using System;

namespace SampleWebApiApplicationWithElasticsearch.Persistence
{
    public interface IPersonRepository : IRepository<PersonEntity, Guid>
    {
    }
}
