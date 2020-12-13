using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Application.Commands;
using Biplov.PaymentGateway.Domain.Entities;
using Biplov.PaymentGateway.Domain.Interfaces;
using Biplov.PaymentGateway.Infrastructure.Idempotency;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Biplov.PaymentGateway.Application.CommandHandlers
{
    public class CreateMerchantCommandHandler : IRequestHandler<CreateMerchantCommand, Result>
    {
        private readonly IMerchantRepository _merchantRepository;

        public CreateMerchantCommandHandler(IMerchantRepository merchantRepository)
        {
            _merchantRepository = merchantRepository;
        }

        public async Task<Result> Handle(CreateMerchantCommand request, CancellationToken cancellationToken)
        {
            var merchant = await _merchantRepository.GetByIdentityIdAsync(request.MerchantId);
            if (merchant is { }) return Result.Ok();

            var newMerchant = new Merchant(request.MerchantId, request.MerchantEmail, request.MerchantName, request.PublicKey, request.PrivateKey, 
                request.SupportedCurrencies.ToList(), request.CommandId);
            _merchantRepository.Add(newMerchant);
            var result = await _merchantRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return result.IsSuccess ? Result.Ok() : Result.Fail(result.Error);
        }
    }

    public class CreateMerchantIdentifiedCommandHandler : IdentifiedCommandHandler<CreateMerchantCommand, Result>
    {
        public CreateMerchantIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager, 
            ILogger<IdentifiedCommandHandler<CreateMerchantCommand, Result>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override Result CreateResultForDuplicateRequest() => Result.Ok();
    }
}
