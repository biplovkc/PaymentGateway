namespace Biplov.BankService
{
    public class Card
    {
        public string Name { get;set; }

        public string Number { get;set; }

        public int ExpiryMonth { get;set; }

        public int ExpiryYear { get;set; }

        public string Cvv { get;set; }
    }
}