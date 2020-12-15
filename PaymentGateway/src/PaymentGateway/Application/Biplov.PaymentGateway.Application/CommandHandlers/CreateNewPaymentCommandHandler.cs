using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Application.Commands;
using Biplov.PaymentGateway.Application.Constants;
using Biplov.PaymentGateway.Application.Queries;
using Biplov.PaymentGateway.Domain.Entities;
using Biplov.PaymentGateway.Domain.Interfaces;
using Biplov.PaymentGateway.Infrastructure.Idempotency;
using Biplov.RiskAnalysis;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Biplov.PaymentGateway.Application.CommandHandlers
{
    public class CreateNewPaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICardQuery _cardQuery;
        private readonly IMerchantQuery _merchantQuery;
        private readonly IRiskAnalysisService _riskAnalysisService;
        public CreateNewPaymentCommandHandler(IPaymentRepository paymentRepository, IMerchantQuery merchantQuery, ICardQuery cardQuery, IRiskAnalysisService riskAnalysisService)
        {
            _paymentRepository = paymentRepository;
            _merchantQuery = merchantQuery;
            _cardQuery = cardQuery;
            _riskAnalysisService = riskAnalysisService;
        }

        public async Task<Result> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            // Check if currency is supported for given merchant
            var isCurrencySupported = await _merchantQuery.IsCurrencySupported(request.MerchantId, request.Currency);
            if (!isCurrencySupported)
                return Result.Fail(ExternalErrorReason.CurrencyNotSupported);

            // Check if card if valid
            var isCardValid = await _cardQuery.IsCardValid(request.CardToken, request.Cvv);
            if (!isCardValid)
                return Result.Fail<string>(ExternalErrorReason.InvalidCard);

            // Perform risk analysis
            if (!string.IsNullOrEmpty(request.OriginIp) && IPAddress.TryParse(request.OriginIp, out IPAddress ipAddress))
            {
                var riskAnalysisResult = await _riskAnalysisService.GetRiskAnalysis(ipAddress);
                if (!riskAnalysisResult.IsSuccess)
                    return Result.Fail<string>(riskAnalysisResult.Error);
            }
            var payment = new Payment(request.MerchantId, request.Currency, request.Amount, request.Reference, request.OriginIp, 
                request.Description);
            payment.SetCardPaymentSource(request.CardToken, request.Cvv);
            if (request.Shipping != null)
                payment.AddBillingAddress(request.Shipping.AddressLine1, request.Shipping.AddressLine2, request.Shipping.City,
                    request.Shipping.Zip, request.Shipping.State, request.Shipping.Country);
            if(request.Recipient != null)
                payment.SetPaymentRecipient(DateTime.Parse(request.Recipient.DateOfBirth), request.Recipient.AccountNumber, request.Recipient.FirstName,
                    request.Recipient.LastName, request.Recipient.Zip);
            payment.SetNotificationsForMerchant(request.SuccessUrl, request.ErrorUrl);
            payment.InitiatePayment();

            _paymentRepository.Add(payment);

            var persistenceResult = await _paymentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return persistenceResult.IsSuccess
                ? Result.Ok(payment.PaymentId)
                : Result.Fail<string>(persistenceResult.Error);
        }
    }

    public class CreateNewPaymentIdentifiedCommandHandler : IdentifiedCommandHandler<CreatePaymentCommand, Result>
    {
        public CreateNewPaymentIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager, ILogger<IdentifiedCommandHandler<CreatePaymentCommand, Result>> logger) 
            : base(mediator, requestManager, logger)
        {
        }
        protected override Result CreateResultForDuplicateRequest() => Result.Ok();

    }
}
