using System.Collections.Generic;
using Biplov.Common.Core;

namespace Biplov.PaymentGateway.Domain.Entities
{
    public class IdPaymentSource : ValueObject
    {
        private IdPaymentSource()
        {
        }

        public IdPaymentSource(string cardToken, string cvv)
        {
            CardToken = cardToken;
            Cvv = cvv;
        }

        /// <summary>
        /// If a card is stored then its identifier can be used as payment source
        /// </summary>
        public string CardToken { get; }

        public string Cvv { get; }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return CardToken;

            yield return Cvv;
        }
    }
}
