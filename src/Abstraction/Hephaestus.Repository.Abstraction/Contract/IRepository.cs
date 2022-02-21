using Hephaestus.Repository.Abstraction.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Abstraction.Contract
{
    public interface IRepository<TEntity, TKey> where TEntity : Entity, IAggregateRoot
    {
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);

        Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetListAsync(CancellationToken cancellationToken);
    }
}
