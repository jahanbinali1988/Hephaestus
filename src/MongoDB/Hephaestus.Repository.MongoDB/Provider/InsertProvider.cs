using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.MongoDB.Provider
{
    internal class InsertProvider<T> : ICommandProvider<T> where T : Entity
    {
        private readonly IMongoCollection<T> _collection;
        internal InsertProvider(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public async Task ExecuteAsync(EntityContextInfo<T> contextInfo, CancellationToken token)
        {
            await _collection.InsertOneAsync(contextInfo.Document, new InsertOneOptions() { BypassDocumentValidation = true }, token);
        }
    }
}
