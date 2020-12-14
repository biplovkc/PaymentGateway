namespace Biplov.MerchantIdentity.Application.Response
{
    public class CreateMerchantResponse
    {
        /// <summary>
        /// Merchant Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Public Key
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// Private/Secret key. Should not be share with anyone.
        /// </summary>
        public string PrivateKey { get; set; }
    }
}
