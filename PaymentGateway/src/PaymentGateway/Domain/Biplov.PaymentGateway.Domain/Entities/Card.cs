using System.Text.RegularExpressions;
using Biplov.Common.Core;

namespace Biplov.PaymentGateway.Domain.Entities
{
    public class Card : Entity
    {
        private Card(){}

        public string CardToken { get; } 

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

        public string MaskedCardNumber { get; }

        public Card(string number, int expiryMonth, int expiryYear, string name, string cvv)
        {
            Number = number;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            Name = name;
            Cvv = cvv;
            CardToken = $"cardTok_{Id:N}";
            MaskedCardNumber = Regex.Replace(Number, "[0-9](?=[0-9]{4})", "*");
        }
    }
}
