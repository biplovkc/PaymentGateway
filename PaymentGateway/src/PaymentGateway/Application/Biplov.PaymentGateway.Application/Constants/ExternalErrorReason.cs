namespace Biplov.PaymentGateway.Application.Constants
{
    public class ExternalErrorReason
    {
        public const string InvalidCvv = "cvv_number_must_between_3_or_4_numbers";
        public const string InvalidCardHolderName = "card_holder_name_invalid";
        public const string InvalidCardExpiryYear = "card_expiry_year_invalid";
        public const string InvalidCardExpiryMonth = "card_expiry_month_inavlid";
        public const string InvalidCardNumber = "card_number_invalid";
        public const string CardValidationFailedByBank = "card_was_flagged_invalid_by_bank";

        public const string InvalidPaymentAmount = "payment_amount_invalid";
        public const string InvalidCurrency = "currency_invalid";
        public const string InvalidRecepientAccountNumber = "recipient_account_number_invalid";
        public const string InvalidRecipientDateOfBirth = "recipient_date_of_birth_invalid";
        public const string InvalidRecipientFirstName = "recipient_first_name_invalid";
        public const string InvalidRecipientLastName = "recipient_last_name_invalid";
        public const string InvalidCardToken = "card_token_invalid";
        public const string InvalidSecretKey = "secret_key_invalid";
        public const string CurrencyNotSupported = "currency_not_supported";
        public const string InvalidCard = "card_is_invalid";
        public const string PaymentNotFound = "payment_not_found";
    }
}
