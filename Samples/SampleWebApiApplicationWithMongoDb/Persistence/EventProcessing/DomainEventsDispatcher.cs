using Hephaestus.Repository.Abstraction.EventProcessing.DomainEvent;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApiApplicationWithMongoDb.Persistence.EventProcessing
{
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IMediator _mediator;
        private readonly SampleMongoDbContext _dbContext;

        public DomainEventsDispatcher(IMediator mediator, SampleMongoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task DispatchEventsAsync()
        {
            var tasks = this._dbContext.GetDomainEvents()
                .Select(x => _mediator.Publish(x))
                .ToList();

            await Task.WhenAll(tasks.ToArray());

            var elasticChange = _dbContext.GetDomainEvents();
            if (elasticChange != null && elasticChange.Count != 0)
                await Task.WhenAll(elasticChange.Select(s => _mediator.Publish(s)));
        }
    }
}
