using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Biplov.PaymentGateway.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Biplov.PaymentGatewayApi.Filters
{
    public class AuthenticateAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var isAuthorizationAvailable =
                context.HttpContext.Request.Headers.TryGetValue("Authorization", out var secretKey);
            if (!isAuthorizationAvailable)
            {
                context.Result = new UnauthorizedObjectResult("unauthorized");
                return;
            }

            if (secretKey.ToString().Substring(0,3).ToLower() != "sk_")
            {
                context.Result = new UnauthorizedObjectResult("invalid_authorization_key");
                return;
            }
        }
    }
}
