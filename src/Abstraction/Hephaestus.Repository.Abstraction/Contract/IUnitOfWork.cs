using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Abstraction.Contract
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
