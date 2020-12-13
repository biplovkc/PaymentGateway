using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Application.Commands;
using Biplov.PaymentGateway.Application.Request;
using Biplov.PaymentGateway.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Biplov.PaymentGatewayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> TokenizeCard([FromBody] TokenizeCardRequest request)
        {
            var innerCommand = new AddNewCardCommand(request.Name, request.Number, request.ExpiryMonth, request.ExpiryYear, request.Cvv, null, HttpContext.TraceIdentifier);
            var command = new IdentifiedCommand<AddNewCardCommand, Result<CreateCardResponse>>(innerCommand);
            var result = await _mediator.Send(command);
            return Ok(result.Value);
        }
    }
}
