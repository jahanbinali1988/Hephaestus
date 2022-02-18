using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.Elasticsearch.Extensions;
using Nest;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Elasticsearch.Provider
{
    public class DeleteProvider : ICommandProvider
    {
        private readonly IElasticClient _elasticClient;

        public DeleteProvider(IElasticClient elasticClient)
        {
            this._elasticClient = elasticClient;
        }

        public async Task ExecuteAsync(EntityContextInfo entity, CancellationToken token)
        {
            var deleteResponse = await _elasticClient.DeleteAsync(new DeleteRequest(entity.EntityType, new Id(entity.Id)), token).ConfigureAwait(true);
            deleteResponse.EnsureRequestSuccess();
        }
    }
}
