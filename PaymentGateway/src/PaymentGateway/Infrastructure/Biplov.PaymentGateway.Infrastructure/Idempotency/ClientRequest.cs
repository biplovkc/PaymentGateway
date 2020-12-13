using System;

namespace Biplov.PaymentGateway.Infrastructure.Idempotency
{
    public class ClientRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
    }
}
