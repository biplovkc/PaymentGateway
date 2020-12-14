using System;
using Biplov.PaymentGateway.Domain.Enum;

namespace Biplov.PaymentGateway.Application.Dtos
{
    public class PaymentDto
    {
        public string PaymentId { get; set; }

        public PaymentStatus Status { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string PaymentSource { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
