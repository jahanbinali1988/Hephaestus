using Hephaestus.Repository.Abstraction.Contract;
using System.Threading.Tasks;
using System.Threading;

namespace Hephaestus.Repository.MongoDB.Contract
{
    public interface IMongoDbUnitOfWork : IUnitOfWork
    {
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
