using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.Abstraction.EventProcessing.DomainEvent;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace SampleWebApiApplicationWithMongoDb.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly IDomainEventsDispatcher _domainEventsDispatcher;
        private readonly SampleMongoDbContext _dbContext;
        private IClientSessionHandle _session;
        public UnitOfWork(IDomainEventsDispatcher domainEventsDispatcher, SampleMongoDbContext dbContext)
        {
            _domainEventsDispatcher = domainEventsDispatcher;
            _dbContext = dbContext;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _session = await this._dbContext.StartSessionAsync(cancellationToken);
            _session.StartTransaction();
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _session.CommitTransactionAsync(cancellationToken);
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _session.AbortTransactionAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _domainEventsDispatcher.DispatchEventsAsync();
            await _dbContext.SaveChangesAsync(cancellationToken);
            _dbContext.ClearChanges();
        }

        public void ClearChanges()
        {
            _dbContext.ClearChanges();
        }
    }
}
