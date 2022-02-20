using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.Elasticsearch.Extensions;
using Nest;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Elasticsearch.Provider
{
    internal class InsertProvider<T> : ICommandProvider<T> where T : Entity
    {
        private readonly IElasticClient elasticClient;
        internal InsertProvider(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task ExecuteAsync(EntityContextInfo<T> entity, CancellationToken token)
        {
            var upsertResponse = await elasticClient.IndexAsync(entity.Document, u => u.Index(entity.EntityType), token).ConfigureAwait(true);
            upsertResponse.EnsureRequestSuccess();
        }
    }
}
