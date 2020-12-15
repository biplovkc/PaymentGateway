using System;
using System.Runtime.Serialization;
using Biplov.Common.Core;
using Biplov.MerchantIdentity.Application.Response;
using MediatR;

namespace Biplov.MerchantIdentity.Application.Commands
{
    [DataContract]
    public class CreateMerchantCommand: IRequest<Result<CreateMerchantResponse>>
    {
        [DataMember]
        public string Name { get; }

        [DataMember]
        public string Email { get; }

        [DataMember]
        public string SupportedCurrencies { get; }

        [DataMember]
        public string CommandId => $"create_merchant_{Name}_{DateTimeOffset.Now.ToUnixTimeSeconds()}";

        [DataMember]
        public string CorrelationId { get; }
        public CreateMerchantCommand(string name, string email, string supportedCurrencies, string correlationId = null) 
        {
            Name = name;
            Email = email.ToLower();
            SupportedCurrencies = supportedCurrencies;
            CorrelationId = correlationId;
        }
    }
}
