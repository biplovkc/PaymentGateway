namespace Biplov.PaymentGateway.Application.Response
{
    public class CreateCardResponse
    {
        /// <summary>
        /// To be used while making payment request
        /// </summary>
        public string CardToken { get; set; }

        /// <summary>
        /// Masked card number, displaying last 4 digits of the card
        /// </summary>
        public string MaskedCardNumber { get; set; }
    }
}
