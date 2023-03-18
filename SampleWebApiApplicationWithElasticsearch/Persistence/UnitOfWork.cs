using Hephaestus.Repository.Abstraction.EventProcessing.DomainEvent;
using Hephaestus.Repository.Elasticsearch.Contract;
using System.Threading;
using System.Threading.Tasks;

namespace SampleWebApiApplicationWithElasticsearch.Persistence
{
    public class UnitOfWork : IElasticsearchUnitOfWork
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
    }
}
