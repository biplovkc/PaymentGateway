using System.Net;
using System.Threading.Tasks;
using Biplov.MerchantIdentity.Application.Commands;
using Biplov.MerchantIdentity.Application.Requests;
using Biplov.MerchantIdentity.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Biplov.MerchantIdentityApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MerchantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MerchantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new merchant
        /// </summary>
        /// <param name="request">Create merchant request</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreateMerchantResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]

        public async Task<IActionResult> CreateMerchant([FromBody] CreateMerchantRequest request)
        {
            var command = new CreateMerchantCommand(request.Name, request.Email, request.SupportedCurrencies, HttpContext.TraceIdentifier);
            var result = await _mediator.Send(command);

            return result.IsSuccess
                ? (IActionResult) Ok(result.Value)
                : UnprocessableEntity();
        }
    }
}
