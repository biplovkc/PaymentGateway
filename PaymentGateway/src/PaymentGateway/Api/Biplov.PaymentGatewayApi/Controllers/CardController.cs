using System;
using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Application.Commands;
using Biplov.PaymentGateway.Application.Constants;
using Biplov.PaymentGateway.Application.Queries;
using Biplov.PaymentGateway.Application.Request;
using Biplov.PaymentGatewayApi.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Biplov.PaymentGatewayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authenticate]
    public class CardController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMerchantQuery _merchantQuery;
        public CardController(IMediator mediator, IMerchantQuery merchantQuery)
        {
            _mediator = mediator;
            _merchantQuery = merchantQuery;
        }

        [HttpPost]
        public async Task<IActionResult> TokenizeCard([FromBody] TokenizeCardRequest request)
        {
            var merchantIdentity = await _merchantQuery.GetMerchantIdAsync(GetAuthorizationKey());
            if (merchantIdentity.Equals(Guid.Empty))
                return new UnauthorizedObjectResult(ExternalErrorReason.InvalidSecretKey);

            var innerCommand = new AddNewCardCommand(request.Name, request.Number, request.ExpiryMonth, request.ExpiryYear, request.Cvv, null, HttpContext.TraceIdentifier);
            var command = new IdentifiedCommand<AddNewCardCommand, Result>(innerCommand);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
