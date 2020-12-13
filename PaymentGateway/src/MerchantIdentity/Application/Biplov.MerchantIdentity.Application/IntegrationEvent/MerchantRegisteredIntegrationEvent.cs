using System;
using System.Collections.Generic;

namespace Biplov.MerchantIdentity.Application.IntegrationEvent
{
    public class MerchantRegisteredIntegrationEvent : EventBus.Events.IntegrationEvent
    {
        public Guid MerchantId { get; }
        public string MerchantName { get; }
        public string MerchantEmail { get; }
        public string PublicKey { get; }
        public string PrivateKey { get; }
        public string CommandId { get; }
        public IEnumerable<string> SupportedCurrencies { get; }

        public MerchantRegisteredIntegrationEvent(Guid merchantId, string merchantName, string merchantEmail, 
            string publicKey, string privateKey, IEnumerable<string> supportedCurrencies, string commandId = null)
        {
            MerchantId = merchantId;
            MerchantName = merchantName;
            MerchantEmail = merchantEmail;
            SupportedCurrencies = supportedCurrencies;
            PublicKey = publicKey;
            PrivateKey = privateKey;
            CommandId = commandId;
        }
    }
}
