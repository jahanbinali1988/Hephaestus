using Hephaestus.Repository.Abstraction.Base;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Elasticsearch
{
	internal class ElsticDbCommandDispatcher
	{
        public async Task DispatchAsync(EntityContextInfo<Entity> entityContext, CancellationToken cancellationToken)
        {
            await entityContext.CommandProvider.ExecuteAsync(entityContext, cancellationToken);
        }
    }
}
