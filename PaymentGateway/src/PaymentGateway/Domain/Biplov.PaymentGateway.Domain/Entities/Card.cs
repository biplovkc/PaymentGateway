using System;
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


        // TODO : Add validations accordingly
        public Card(string number, int expiryMonth, int expiryYear, string name, string cvv)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentNullException(nameof(number));

            if (expiryYear < DateTime.Now.Year)
                throw  new ArgumentException("card_expired");

            if (!(cvv.Length == 3 || cvv.Length == 4))
                throw new ArgumentException("invalid_cvv");

            Number = number;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            Name = name;
            Cvv = cvv;
            CardToken = $"cardtok_{Id:N}".ToLower();
            MaskedCardNumber = Regex.Replace(Number, "[0-9](?=[0-9]{4})", "*");
        }
    }
}
