using System;
using System.Threading.Tasks;

using Biplov.Common.Core;
using Biplov.PaymentGateway.Application.Commands;
using Biplov.PaymentGateway.Application.Constants;
using Biplov.PaymentGateway.Application.Queries;
using Biplov.PaymentGateway.Application.Request;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Biplov.PaymentGatewayApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMerchantQuery _merchantQuery;

        public PaymentController(IMediator mediator, IMerchantQuery merchantQuery)
        {
            _mediator = mediator;
            _merchantQuery = merchantQuery;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            var merchantIdentity = await _merchantQuery.GetMerchantIdAsync(GetAuthorizationKey());
            if (merchantIdentity.Equals(Guid.Empty))
                return new UnauthorizedObjectResult(ExternalErrorReason.InvalidSecretKey);

            var innerCommand = new CreatePaymentCommand(merchantIdentity.Value, request.CardToken, request.Cvv, request.Amount, request.Currency, 
                request.Shipping, request.Recipient, request.Reference, request.Description, request.OriginIp, request.SuccessUrl, request.ErrorUrl, request.MetaData,
                HttpContext.TraceIdentifier);
            var command = new IdentifiedCommand<CreatePaymentCommand, Result>(innerCommand);

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
