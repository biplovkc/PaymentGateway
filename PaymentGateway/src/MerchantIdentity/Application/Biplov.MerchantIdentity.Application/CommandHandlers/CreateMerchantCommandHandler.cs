using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.MerchantIdentity.Application.Commands;
using Biplov.MerchantIdentity.Application.Response;
using Biplov.MerchantIdentity.Domain.Entities;
using Biplov.MerchantIdentity.Domain.Interfaces;
using MediatR;

namespace Biplov.MerchantIdentity.Application.CommandHandlers
{
    public class CreateMerchantCommandHandler : IRequestHandler<CreateMerchantCommand, Result<CreateMerchantResponse>>
    {
        private readonly IMerchantIdentityRepository _identityRepository;

        public CreateMerchantCommandHandler(IMerchantIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        public async Task<Result<CreateMerchantResponse>> Handle(CreateMerchantCommand request, CancellationToken cancellationToken)
        {
            var merchant = await _identityRepository.GetByEmailAsync(request.Email);
            if (merchant != null)
                return Result.Fail<CreateMerchantResponse>("merchant_already_exists");
            var newMerchant = _identityRepository.Add(new Merchant(request.Name, request.Email,
                request.SupportedCurrencies.Split(',').ToList(),
                request.CommandId));

            var persistenceResult = await _identityRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            if (persistenceResult.IsSuccess)
            {
                return Result<CreateMerchantResponse>.Ok(new CreateMerchantResponse
                {
                    Id = newMerchant.Id.ToString(),
                    PrivateKey = newMerchant.PrivateKey,
                    PublicKey = newMerchant.PublicKey
                });
            }

            return Result.Fail<CreateMerchantResponse>(persistenceResult.Error);
        }
    }
}
