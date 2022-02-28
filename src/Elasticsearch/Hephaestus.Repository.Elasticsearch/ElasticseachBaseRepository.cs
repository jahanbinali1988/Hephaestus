using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.Elasticsearch.Extensions;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Elasticsearch
{
    public abstract class ElasticseachBaseRepository<T, TKey> : IRepository<T, TKey> where T : Entity, IAggregateRoot
    {
        protected IElasticClient _elasticClient;
        protected ElasticDbContext dbContext;
        protected ElasticseachBaseRepository(ElasticDbContext dbContext)
        {
            this.dbContext = dbContext;
            this._elasticClient = dbContext.GetClient();
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await dbContext.AddDocumentAsync<T>(entity);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            await dbContext.DeleteDocumentAsync<T>(entity);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            await dbContext.UpdateDocumentAsync<T>(entity);
        }

        public async Task<T> GetAsync(TKey id, CancellationToken cancellationToken)
        {
            var response = await _elasticClient.SearchAsync<T>(p => p
                        .Size(1)
                        .Query(q => q
                            .Bool(b => b.
                                Must(m => m.
                                    Term(t => t.
                                        Field(f => f.Id).
                                        Value(id))
                        ))));
            response.EnsureRequestSuccess();
            return response.Documents.FirstOrDefault();
        }

        public async Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken)
        {
            var response = await _elasticClient.SearchAsync<T>(c => c
                .Source(x => x.IncludeAll())
                .Skip(0)
                .Take(10), cancellationToken);
            response.EnsureRequestSuccess();

            return response.Documents;
        }

    }
}
