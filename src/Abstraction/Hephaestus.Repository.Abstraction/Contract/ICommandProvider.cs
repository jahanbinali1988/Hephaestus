using Hephaestus.Repository.Abstraction.Base;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Abstraction.Contract
{
    public interface ICommandProvider<T> where T : Entity
    {
        public Task ExecuteAsync(EntityContextInfo<T> context, CancellationToken token);
    }
}
