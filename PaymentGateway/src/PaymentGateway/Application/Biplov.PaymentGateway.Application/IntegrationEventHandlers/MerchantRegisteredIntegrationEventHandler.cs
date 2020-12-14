using System;
using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.EventBus.Abstractions;
using Biplov.PaymentGateway.Application.Commands;
using Biplov.PaymentGateway.Application.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Biplov.PaymentGateway.Application.IntegrationEventHandlers
{
    public class MerchantRegisteredIntegrationEventHandler : IIntegrationEventHandler<MerchantRegisteredIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MerchantRegisteredIntegrationEventHandler> _logger;

        public MerchantRegisteredIntegrationEventHandler(IMediator mediator, ILogger<MerchantRegisteredIntegrationEventHandler> logger)
        {
            _mediator = mediator?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(MerchantRegisteredIntegrationEvent @event)
        {
            var innerCommand = new CreateMerchantCommand(@event.MerchantId, @event.MerchantName, @event.MerchantEmail,
                @event.PublicKey, @event.PrivateKey, @event.SupportedCurrencies,@event.CommandId);
            var command = new IdentifiedCommand<CreateMerchantCommand, Result>(innerCommand);
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                _logger.LogWarning("Error handling CreateMerchantCommand for integration event : {eventId} and merchant : {id}",@event.Id, @event.MerchantId);
        }
    }
}
