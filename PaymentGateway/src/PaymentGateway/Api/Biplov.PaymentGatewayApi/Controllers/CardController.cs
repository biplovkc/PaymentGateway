using System;
using System.Net;
using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Application.Commands;
using Biplov.PaymentGateway.Application.Constants;
using Biplov.PaymentGateway.Application.Queries;
using Biplov.PaymentGateway.Application.Request;
using Biplov.PaymentGateway.Application.Response;
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
        private readonly ICardQuery _cardQuery;
        public CardController(IMediator mediator, IMerchantQuery merchantQuery, ICardQuery cardQuery)
        {
            _mediator = mediator;
            _merchantQuery = merchantQuery;
            _cardQuery = cardQuery;
        }

        /// <summary>
        /// Add a new card / tokenize card
        /// </summary>
        /// <param name="request">Tokenize card request</param>
        /// <returns></returns>
        [HttpPost("tokenizeCard")]
        [ProducesResponseType(typeof(CreateCardResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> TokenizeCard([FromBody] TokenizeCardRequest request)
        {
            var merchantIdentity = await _merchantQuery.GetMerchantIdAsync(GetAuthorizationKey());
            if (merchantIdentity.Equals(Guid.Empty))
                return new UnauthorizedObjectResult(ExternalErrorReason.InvalidSecretKey);

            var innerCommand = new AddNewCardCommand(request.Name, request.Number, request.ExpiryMonth, request.ExpiryYear, request.Cvv, null, HttpContext.TraceIdentifier);
            var command = new IdentifiedCommand<AddNewCardCommand, Result>(innerCommand);
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                var card = await _cardQuery.GetCardByCardNumberAsync(request.Number);
                return Ok(card);
            }
            return UnprocessableEntity(result.Error);
        }
    }
}
