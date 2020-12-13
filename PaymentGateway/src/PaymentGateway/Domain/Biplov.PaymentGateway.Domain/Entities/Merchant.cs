using System;
using System.Collections.Generic;
using System.Linq;
using Biplov.Common.Core;

namespace Biplov.PaymentGateway.Domain.Entities
{
    public class Merchant : Entity
    {
        private Merchant(){}

        public Guid MerchantIdentity { get; }

        public string Email { get; }

        public string Name { get; }

        public string PublicKey { get; }

        public string PrivateKey { get; }

        private List<string> _supportedCurrencies = new List<string>();

        public IReadOnlyCollection<string> SupportedCurrencies => _supportedCurrencies.AsReadOnly();

        private List<Payment> _payments = new List<Payment>();

        public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();


        public Merchant(Guid merchantIdentity, string email, string name, string publicKey, string privateKey, List<string> supportedCurrencies, string correlationId = null)
        {
            MerchantIdentity = merchantIdentity;
            Email = email;
            Name = name;
            PublicKey = publicKey;
            PrivateKey = privateKey;

            foreach (var supportedCurrency in supportedCurrencies.Where(supportedCurrency => !_supportedCurrencies.Contains(supportedCurrency)))
                _supportedCurrencies.Add(supportedCurrency);
        }
    }
}
