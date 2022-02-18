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

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _domainEventsDispatcher.DispatchEventsAsync();
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
