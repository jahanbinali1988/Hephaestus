using Hephaestus.Repository.Abstraction.EventProcessing.DomainEvent;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApiApplicationWithElasticsearch.Persistence.EventProcessing
{
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IMediator _mediator;
        private readonly SampleElasticDbContext _elasticDbContext;

        public DomainEventsDispatcher(IMediator mediator, SampleElasticDbContext elasticDbContext)
        {
            _mediator = mediator;
            _elasticDbContext = elasticDbContext;
        }

        public async Task DispatchEventsAsync()
        {
            var tasks = this._elasticDbContext.GetDomainEvents()
                .Select(x => _mediator.Publish(x))
                .ToList();

            await Task.WhenAll(tasks.ToArray());

            var elasticChange = _elasticDbContext.GetDomainEvents();
            if (elasticChange != null && elasticChange.Count != 0)
                await Task.WhenAll(elasticChange.Select(s => _mediator.Publish(s)));
        }
    }
}
