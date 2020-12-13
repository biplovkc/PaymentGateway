using System;
using System.Collections.Generic;
using Biplov.Common.Core;
using MediatR;

namespace Biplov.MerchantIdentity.Domain.DomainEvents
{
    public class MerchantAddedDomainEvent : DomainEvent, INotification
    {
        public Guid MerchantId { get; }

        public string MerchantEmail { get; }

        public string PublicKey { get; }

        public string PrivateKey { get; }

        public List<string> Currencies { get; }

        public string MerchantName { get; }

        public string CommandId { get; }

        public MerchantAddedDomainEvent(Guid merchantId, string merchantName, string email, List<string> currencies, string publicKey, string privateKey, string commandId = null)
        {
            MerchantId = merchantId;
            MerchantEmail = email;
            PublicKey = publicKey;
            PrivateKey = privateKey;
            Currencies = currencies;
            CommandId = commandId;
            MerchantName = merchantName;
        }
    }
}
