using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

using Biplov.Common.Core;
using Biplov.Common.Core.Utilities;
using Biplov.MerchantIdentity.Domain.DomainEvents;

namespace Biplov.MerchantIdentity.Domain.Entities
{
    /// <summary>
    /// Merchant (aggregate root) will send payment processing request
    /// </summary>
    public class Merchant : Entity
    {
        private Merchant() { }

        public string Name { get; }

        public string Email { get; }

        private readonly List<string> _supportedCurrencies = new List<string>();

        public IReadOnlyCollection<string> SupportedCurrencies => _supportedCurrencies.AsReadOnly();

        public string PublicKey { get; }
        public string PrivateKey { get; }

        public Merchant(string name, string email, List<string> currencies, string commandId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"invalid {nameof(name)}");

            if (!email.IsValidEmail())
                throw new ArgumentException($"invalid email: {email}");

            Name = name;
            Email = email;
            // Keysize can be adjusted based on requirements
            using var rsa = new RSACryptoServiceProvider(512);
            PublicKey = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());
            PrivateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

            if (currencies.Any(currency => currency.Length != 3))
                throw new ArgumentException($"{nameof(currencies)} invalid");

            _supportedCurrencies.AddRange(currencies.ConvertAll(x => x.ToUpper()));

            RaiseDomainEvent(new MerchantAddedDomainEvent(Id, name, email, currencies, PublicKey, PrivateKey, commandId));
        }

        public void AddCurrencySupport(string currency, string commandId = null)
        {
            if (string.IsNullOrWhiteSpace(currency) || currency.Length !=3)
                throw new ArgumentException($"invalid currency code: {currency}");

            if (! _supportedCurrencies.Contains(currency))
                _supportedCurrencies.Add(currency);

            RaiseDomainEvent(new CurrencySupportAddedDomainEvent(Id, currency, commandId));
        }



        public void RemoveCurrencySupport(string currency, string commandId = null)
        {
            if (!_supportedCurrencies.Contains(currency)) 
                return;
            
            _supportedCurrencies.Remove(currency);
            RaiseDomainEvent(new CurrencySupportRevokedDomainEvent(Id, currency, commandId));
        }
    }
}
