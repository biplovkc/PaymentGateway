using System;
using System.Net;
using System.Threading.Tasks;

using Biplov.Common.Core;
using Biplov.Common.Core.Extensions;
using Biplov.PaymentGateway.Application.Commands;
using Biplov.PaymentGateway.Application.Constants;
using Biplov.PaymentGateway.Application.Dtos;
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
        private readonly IPaymentQuery _paymentQuery;

        public PaymentController(IMediator mediator, IMerchantQuery merchantQuery, IPaymentQuery paymentQuery)
        {
            _mediator = mediator;
            _merchantQuery = merchantQuery;
            _paymentQuery = paymentQuery;
        }

        /// <summary>
        /// Get payment by payment identifier
        /// </summary>
        /// <param name="paymentId">paymentIdentifier</param>
        /// <param name="fields">fields to select from result</param>
        /// <returns>Payment dto</returns>
        [HttpGet("{paymentId}", Name = "GetPayment")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaymentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]        
        public async Task<IActionResult> GetPayment(string paymentId, [FromQuery]string fields)
        {
            var payment = await _paymentQuery.GetPaymentInfoAsync(paymentId);

            if (payment.IsSuccess)
                return Ok(payment.Value.ShapeData(fields));

            if (!payment.IsSuccess && payment.Error.Equals(ExternalErrorReason.PaymentNotFound))
                return NotFound(paymentId);

            return UnprocessableEntity(payment.Error);
        }

        /// <summary>
        /// Create a new payment
        /// </summary>
        /// <param name="request">create payment request</param>
        /// <returns>route pointing payment detail info</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            var merchantIdentity = await _merchantQuery.GetMerchantIdAsync(GetAuthorizationKey());
            if (merchantIdentity.Equals(Guid.Empty))
                return new UnauthorizedObjectResult(ExternalErrorReason.InvalidSecretKey);

            var innerCommand = new CreatePaymentCommand(merchantIdentity, request.CardToken, request.Cvv, request.Amount, request.Currency, 
                request.Shipping, request.Recipient, request.Reference, request.Description, request.OriginIp, request.SuccessUrl, request.ErrorUrl, request.MetaData,
                HttpContext.TraceIdentifier);
            var command = new IdentifiedCommand<CreatePaymentCommand, Result>(innerCommand);

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return UnprocessableEntity(result.Error);

            return CreatedAtRoute("GetPayment", routeValues: new {paymentId = result.SuccessResult},null);
        }
    }
}
