using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.Elasticsearch.Extensions;
using Nest;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Elasticsearch.Provider
{
    internal class UpdateProvider<T> : ICommandProvider<T> where T : Entity
    {
        private readonly IElasticClient _elasticClient;
        internal UpdateProvider(IElasticClient elasticClient)
        {
            this._elasticClient = elasticClient;
        }

        public async Task ExecuteAsync(EntityContextInfo<T> entity, CancellationToken token)
        {
            var insertResponse = await _elasticClient.IndexAsync(entity.Document, u => u.Index(entity.EntityType), token).ConfigureAwait(true);
            insertResponse.EnsureRequestSuccess();
        }
    }
}
