using System.Runtime.Serialization;
using MediatR;

namespace Biplov.PaymentGateway.Application.Request
{
    [DataContract]
    public class TokenizeCardRequest : IRequest<bool>
    {
        [DataMember]
        public string Name { get;set; }

        [DataMember]
        public string Number { get;set; }

        [DataMember]
        public int ExpiryMonth { get;set; }

        [DataMember]
        public int ExpiryYear { get;set; }

        [DataMember]
        public string Cvv { get;set; }
    }
}
