using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.Abstraction.EventProcessing.DomainEvent;
using System.Threading;
using System.Threading.Tasks;

namespace SampleWebApiApplicationWithMongoDb.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;
        private readonly SampleMongoDbContext _dbContext;
        public UnitOfWork(IDomainEventsDispatcher domainEventsDispatcher, SampleMongoDbContext dbContext)
        {
            _domainEventsDispatcher = domainEventsDispatcher;
            _dbContext = dbContext;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _domainEventsDispatcher.DispatchEventsAsync();
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
