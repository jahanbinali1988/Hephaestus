using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.MongoDB
{
    public abstract class MongoDbBaseRepository<T, TKey> : IRepository<T, TKey> where T : Entity, IAggregateRoot
    {
        protected readonly MongoContext Context;
        protected abstract string CollectionName { get; }
        protected IMongoCollection<T> DbSet;
        protected MongoDbBaseRepository(MongoContext context)
        {
            Context = context;
            DbSet = Context.GetCollection<T>(typeof(T).Name);
        }
        public abstract Task<TKey> GetNextId();

        public Task AddAsync(T aggregate, CancellationToken cancellationToken)
        {
            Context.AddDocument<T>(aggregate);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(T aggregate, CancellationToken cancellationToken)
        {
            Context.UpdateDocument<T>(aggregate);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T aggregate, CancellationToken cancellationToken)
        {
            Context.DeleteDocument<T>(aggregate);
            return Task.CompletedTask;
        }

        public async Task<T> GetAsync(TKey key, CancellationToken cancellationToken)
        {
            var data = await DbSet.FindAsync(Builders<T>.Filter.Eq("_id", key));
            return await data.SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken)
        {
            var all = await DbSet.FindAsync(Builders<T>.Filter.Empty);
            return await all.ToListAsync(cancellationToken);
        }
    }
}
