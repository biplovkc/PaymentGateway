using System.Runtime.Serialization;

namespace Biplov.MerchantIdentity.Application.Requests
{
    [DataContract]
    public class CreateMerchantRequest
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string SupportedCurrencies { get; set; }
    }
}
