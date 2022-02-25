using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.MongoDB.Provider
{
    internal class UpdateProvider<T> : ICommandProvider<T> where T : Entity
    {
        private readonly IMongoCollection<T> _collection;
        internal UpdateProvider(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public async Task ExecuteAsync(EntityContextInfo<T> contextInfo, CancellationToken token)
        {
            var builder = Builders<T>.Filter;

            var filter = builder.Eq(e => e.Id, contextInfo.Document.Id);

            await _collection.ReplaceOneAsync(filter, (T)contextInfo.Document, cancellationToken: token);
        }
    }
}
