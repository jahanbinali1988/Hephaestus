using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleWebApiApplicationWithElasticsearch.Shared
{
    public abstract class DomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent> where TDomainEvent : INotification
    {
        private readonly ILogger _logger;

        protected DomainEventHandler(ILogger logger)
        {
            _logger = logger;
        }

        public Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                return HandleEvent(notification, cancellationToken);
            }
            catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                //ignore 
                throw;
            }
            catch (Exception exception)
            {
                exception.Data.Add("NotificationType", typeof(TDomainEvent));
                exception.Data.Add("Notification", JsonConvert.SerializeObject(notification));

                _logger.LogCritical(exception, exception.Message);

                throw;
            }
        }

        protected abstract Task HandleEvent(TDomainEvent notification, CancellationToken cancellationToken);
    }
}
