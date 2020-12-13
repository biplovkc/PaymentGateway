namespace Biplov.PaymentGateway.Domain.Entities
{
    public class IdPaymentSource : PaymentSource
    {
        private IdPaymentSource(){}
        /// <summary>
        /// If a card is stored then its identifier can be used as payment source
        /// </summary>
        public string Id { get; }

        public string Cvv { get; }
    }
}
