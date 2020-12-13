using System.Collections.Generic;
using Biplov.Common.Core;

namespace Biplov.PaymentGateway.Domain.Entities
{
    public class Customer : ValueObject
    {
        private Customer(){}
        public string Email { get; }

        public string Name { get; }

        public string Reference { get; }

        public Customer(string email, string name, string reference)
        {
            Email = email;
            Name = name;
            Reference = reference;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Email;
            yield return Name;
            yield return Reference;
        }
    }
}
