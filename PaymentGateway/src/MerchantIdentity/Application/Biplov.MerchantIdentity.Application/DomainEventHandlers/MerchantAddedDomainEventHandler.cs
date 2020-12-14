using System;
using System.Threading;
using System.Threading.Tasks;
using Biplov.EventBus.Abstractions;
using Biplov.MerchantIdentity.Application.IntegrationEvent;
using Biplov.MerchantIdentity.Domain.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Biplov.MerchantIdentity.Application.DomainEventHandlers
{
    public class MerchantAddedDomainEventHandler : INotificationHandler<MerchantAddedDomainEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<MerchantAddedDomainEventHandler> _logger;

        public MerchantAddedDomainEventHandler(IEventBus eventBus, ILogger<MerchantAddedDomainEventHandler> logger)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(MerchantAddedDomainEvent notification, CancellationToken cancellationToken)
        {
            var @event = new MerchantRegisteredIntegrationEvent(notification.MerchantId, notification.MerchantName,
                notification.MerchantEmail,
                notification.PublicKey, notification.PrivateKey, notification.Currencies, notification.CommandId);
            _eventBus.Publish(@event);
            return Task.CompletedTask;
        }
    }
}
