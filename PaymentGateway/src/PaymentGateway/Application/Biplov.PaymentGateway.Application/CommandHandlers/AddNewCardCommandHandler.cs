using System.Threading;
using System.Threading.Tasks;
using Biplov.BankService;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Application.Commands;
using Biplov.PaymentGateway.Application.Constants;
using Biplov.PaymentGateway.Application.Response;
using Biplov.PaymentGateway.Domain.Entities;
using Biplov.PaymentGateway.Domain.Interfaces;
using Biplov.PaymentGateway.Infrastructure.Idempotency;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Biplov.PaymentGateway.Application.CommandHandlers
{
    public class AddNewCardCommandHandler : IRequestHandler<AddNewCardCommand, Result>
    {
        private readonly ICardRepository _cardRepository;
        private readonly IBankService _bankService;
        private readonly ILogger<AddNewCardCommandHandler> _logger;

        public AddNewCardCommandHandler(ICardRepository cardRepository, IBankService bankService, ILogger<AddNewCardCommandHandler> logger)
        {
            _cardRepository = cardRepository;
            _bankService = bankService;
            _logger = logger;
        }

        public async Task<Result> Handle(AddNewCardCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending card validation request for cardNumber : {cardNumber} with correlation id : {correlationId} to bank ",request.Number, request.CorrelationId);
            var bankResult = await _bankService.ValidateCard(request.Name, request.Number, request.ExpiryMonth,
                request.ExpiryYear, request.Cvv);

            if (!bankResult.IsSuccess)
            {
                _logger.LogInformation("Card was flagged invalid by bank for cardNumber : {cardNumber} for correlationid : {correlationId}", request.Number, request.CorrelationId);
                return Result.Fail<CreateCardResponse>(ExternalErrorReason.CardValidationFailedByBank);
            }
            _logger.LogInformation("card validation request was successful for correlationid : {correlationId}", request.CorrelationId);
            var card = new Card(request.Number, request.ExpiryMonth, request.ExpiryYear, request.Name, request.Cvv);
            _cardRepository.Add(card);
            var persistenceResult = await _cardRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            if (!persistenceResult.IsSuccess) return Result.Fail<CreateCardResponse>(persistenceResult.Error);

            var successResponse = new CreateCardResponse
            {
                CardToken = card.CardToken,
                MaskedCardNumber = card.MaskedCardNumber
            };
            return Result.Ok(successResponse);
        }
    }
    public class AddNewCardIdentifiedCommandHandler : IdentifiedCommandHandler<AddNewCardCommand, Result>
    {
        public AddNewCardIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager, 
            ILogger<IdentifiedCommandHandler<AddNewCardCommand, Result>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override Result CreateResultForDuplicateRequest() => Result.Ok();
    }
}
