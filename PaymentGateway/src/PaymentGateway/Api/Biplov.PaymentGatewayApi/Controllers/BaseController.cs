using Microsoft.AspNetCore.Mvc;

namespace Biplov.PaymentGatewayApi.Controllers
{
    public class BaseController : ControllerBase
    {
        [NonAction]
        protected string GetAuthorizationKey()
        {
            var isPrivateKeyAvailable = HttpContext.Request.Headers.TryGetValue("Authorization", out var privateKey);
            return isPrivateKeyAvailable ? privateKey.ToString() : string.Empty;
        }
    }
}
