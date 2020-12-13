using Biplov.Common.Core;

namespace Biplov.PaymentGateway.Domain.Entities
{
    public class Card : Entity
    {
        private Card(){}

        /// <summary>
        /// Card number
        /// </summary>
        public string Number { get; }

        public int ExpiryMonth { get; }

        public int ExpiryYear { get; }

        /// <summary>
        /// Name of card holder
        /// </summary>
        public string Name { get; }

        public string Cvv { get; }

        public Address BillingAddress { get; private set; }

        protected Card(string number, int expiryMonth, int expiryYear, string name, string cvv)
        {
            Number = number;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            Name = name;
            Cvv = cvv;
        }

        public void SetBillingAddress(string addressLine1, string addressLine2, string city, string zipCode, string state, string country)
        {
            BillingAddress = new Address(addressLine1, addressLine2, city, zipCode, state, country);
        }
    }
}
