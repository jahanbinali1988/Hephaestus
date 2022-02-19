using Microsoft.Extensions.Logging;
using SampleWebApiApplicationWithMongoDb.Models.DomainEvent;
using SampleWebApiApplicationWithMongoDb.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace SampleWebApiApplicationWithMongoDb.Handlers.DomainEventConsumers
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
