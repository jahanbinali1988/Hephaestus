using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.Elasticsearch.Extensions;
using Nest;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Elasticsearch.Provider
{
    internal class DeleteProvider<T> : ICommandProvider<T> where T : Entity
    {
        private readonly IElasticClient _elasticClient;
        internal DeleteProvider(IElasticClient elasticClient)
        {
            this._elasticClient = elasticClient;
        }

        public async Task ExecuteAsync(EntityContextInfo<T> entity, CancellationToken token)
        {
            var deleteResponse = await _elasticClient.DeleteAsync(new DeleteRequest(entity.EntityType, new Id(entity.Document.Id)), token).ConfigureAwait(true);
            deleteResponse.EnsureRequestSuccess();
        }
    }
}
