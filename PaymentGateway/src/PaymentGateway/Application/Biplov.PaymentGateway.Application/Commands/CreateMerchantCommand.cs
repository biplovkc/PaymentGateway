using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Biplov.PaymentGateway.Application.Constants;

namespace Biplov.PaymentGateway.Application.Commands
{
    [DataContract]
    public class CreateMerchantCommand : Command
    {
        [DataMember]
        public Guid MerchantId { get; }

        [DataMember]
        public string MerchantName { get; }

        [DataMember]
        public string MerchantEmail { get; }

        [DataMember]
        public string PublicKey { get; }

        [DataMember]
        public string PrivateKey { get; }

        [DataMember]
        public IEnumerable<string> SupportedCurrencies { get; }

        [DataMember] public override string CommandId => $"{CommandPrefix.CreateMerchant}{MerchantId}";

        public CreateMerchantCommand(Guid merchantId, string merchantName, string merchantEmail, string publicKey, string privateKey, IEnumerable<string> supportedCurrencies, string correlationId = null) : base(correlationId)
        {
            MerchantId = merchantId;
            MerchantName = merchantName;
            MerchantEmail = merchantEmail;
            PublicKey = publicKey;
            PrivateKey = privateKey;
            SupportedCurrencies = supportedCurrencies;
        }
    }
}
