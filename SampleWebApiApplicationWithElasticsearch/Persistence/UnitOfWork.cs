using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.Abstraction.EventProcessing.DomainEvent;
using System.Threading;
using System.Threading.Tasks;

namespace SampleWebApiApplicationWithElasticsearch.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly IDomainEventsDispatcher _domainEventsDispatcher;
        private readonly SampleElasticDbContext _dbContext;
        public UnitOfWork(IDomainEventsDispatcher domainEventsDispatcher, SampleElasticDbContext dbContext)
        {
            _domainEventsDispatcher = domainEventsDispatcher;
            _dbContext = dbContext;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _domainEventsDispatcher.DispatchEventsAsync();
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Transaction in snot supported in this technology
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Transaction in snot supported in this technology
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Transaction in snot supported in this technology
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

    }
}
