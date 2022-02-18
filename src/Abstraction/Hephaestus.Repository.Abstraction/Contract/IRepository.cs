using Hephaestus.Repository.Abstraction.Base;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Abstraction.Contract
{
    public interface IRepository<TEntity, Tkey> where TEntity : Entity<Tkey>, IAggregateRoot
    {
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);

        Task<TEntity> GetAsync(Tkey id, CancellationToken cancellationToken);
    }

    public interface IRepository<TEntity> : IRepository<TEntity, long> where TEntity : Entity, IAggregateRoot
    {

    }
}
