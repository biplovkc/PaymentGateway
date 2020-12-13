using Biplov.PaymentGateway.Domain.Enum;

namespace Biplov.PaymentGateway.Domain.Entities
{
    public abstract class PaymentSource
    {
        public PaymentType Type { get; }
    }
}
