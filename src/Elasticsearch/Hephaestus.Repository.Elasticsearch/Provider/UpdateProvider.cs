using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.Elasticsearch.Extensions;
using Nest;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Elasticsearch.Provider
{
    public class UpdateProvider : ICommandProvider
    {
        private readonly IElasticClient _elasticClient;

        public UpdateProvider(IElasticClient elasticClient)
        {
            this._elasticClient = elasticClient;
        }

        public async Task ExecuteAsync(EntityContextInfo entity, CancellationToken token)
        {
            var insertResponse = await _elasticClient.IndexAsync(entity.Document, u => u.Index(entity.EntityType), token).ConfigureAwait(true);
            insertResponse.EnsureRequestSuccess();
        }
    }
}
