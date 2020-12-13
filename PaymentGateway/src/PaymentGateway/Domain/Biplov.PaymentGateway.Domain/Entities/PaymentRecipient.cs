using System;
using System.Collections.Generic;
using Biplov.Common.Core;

namespace Biplov.PaymentGateway.Domain.Entities
{
    public class PaymentRecipient : ValueObject
    {
        private PaymentRecipient(){}
        public DateTime DateOfBirth { get; }

        public string AccountNumber { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string ZipCode { get; }

        public PaymentRecipient(DateTime dateOfBirth, string accountNumber, string firstName, string lastName, string zipCode)
        {
            DateOfBirth = dateOfBirth;
            AccountNumber = accountNumber;
            FirstName = firstName;
            LastName = lastName;
            ZipCode = zipCode;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return DateOfBirth;
            yield return AccountNumber;
            yield return FirstName;
            yield return LastName;
            yield return ZipCode;
        }
    }
}
