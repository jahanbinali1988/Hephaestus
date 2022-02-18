using Hephaestus.Repository.Abstraction.Base;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Elasticsearch
{
	public class ElsticDbCommandDispatcher
	{
        public async Task DispatchAsync(EntityContextInfo entityContext, CancellationToken cancellationToken)
        {
            await entityContext.CommandProvider.ExecuteAsync(entityContext, cancellationToken);
        }
    }
}
