using Microsoft.Extensions.Logging;
using SampleWebApiApplicationWithElasticsearch.Models.DomainEvent;
using SampleWebApiApplicationWithElasticsearch.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace SampleWebApiApplicationWithElasticsearch.Handlers.DomainEventConsumers
{
    public class CreatePersonDomainEventConsumer : DomainEventHandler<CreatePersonDomainEvent>
    {
        private readonly ILogger<CreatePersonDomainEventConsumer> _logger;
        public CreatePersonDomainEventConsumer(ILogger<CreatePersonDomainEventConsumer> logger) : base(logger)
        {
            _logger = logger;
        }

        protected override async Task HandleEvent(CreatePersonDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Critical, notification.UniqueId.ToString());
        }
    }
}
