using System;
using Biplov.Common.Core;
using MediatR;

namespace Biplov.PaymentGateway.Application.Commands
{
    public abstract class Command : IRequest<Result>
    {
        protected Command( string correlationId = null)
        {
            CorrelationId = correlationId ?? Guid.NewGuid().ToString();
        }

        public virtual string CommandId { get; }
        public virtual string CorrelationId { get; }
    }
}
