namespace Biplov.PaymentGateway.Domain.Entities
{
    public class CardPaymentSource : PaymentSource
    {
        private CardPaymentSource(){}
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

        public string PhoneNumber { get; }

        public CardPaymentSource(string number, int expiryMonth, int expiryYear, string name, string cvv, string phoneNumber)
        {
            // TODO : Add appropriate validation
            Number = number;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            Name = name;
            Cvv = cvv;
            PhoneNumber = phoneNumber;
        }

        public void SetAddress(string addressLine1, string addressLine2, string city, string zipCode, string state, string country)
        {
            // TODO : Add appropriate validation
            BillingAddress = new Address(addressLine1, addressLine2, city, zipCode, state, country);
        }
    }
}
