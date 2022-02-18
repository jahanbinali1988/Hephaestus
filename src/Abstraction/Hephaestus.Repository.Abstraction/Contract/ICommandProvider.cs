using Hephaestus.Repository.Abstraction.Base;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Abstraction.Contract
{
    public interface ICommandProvider
    {
        public Task ExecuteAsync(EntityContextInfo entity, CancellationToken token);
    }
}
