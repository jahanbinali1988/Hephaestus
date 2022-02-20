using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Elasticsearch
{
    public abstract class ElasticseachBaseRepository<T, TKey> : IRepository<T, TKey> where T : Entity, IAggregateRoot
    {
        protected ElasticDbContext dbContext;

        protected ElasticseachBaseRepository(ElasticDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await dbContext.AddDocument<T>(entity);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            await dbContext.DeleteDocument<T>(entity);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            await dbContext.AddDocument<T>(entity);
        }

        public Task<T> GetAsync(TKey id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
