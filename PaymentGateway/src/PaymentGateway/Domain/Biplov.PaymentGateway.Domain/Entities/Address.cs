using System;
using System.Collections.Generic;
using Biplov.Common.Core;

namespace Biplov.PaymentGateway.Domain.Entities
{
    public class Address : ValueObject
    {
        private Address() { }
        public Address(string addressLine1, string addressLine2, string city, string zipCode, string state, string country)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            ZipCode = zipCode;
            State = state;
            Country = country;
        }

        public string AddressLine1 { get; }

        public string AddressLine2 { get;  }

        public string City { get; }

        public string ZipCode { get; }

        public string State { get; }

        public string Country { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return AddressLine1;
            yield return AddressLine2;
            yield return City;
            yield return ZipCode;
            yield return State;
            yield return Country;
        }

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case null:
                    return false;
                case Address business:
                    {
                        var address = business;
                        if (address?.AddressLine1 == AddressLine1 &&
                            address.AddressLine2 == AddressLine2 &&
                            address.City == City && address.ZipCode == ZipCode &&
                            address.State == State &&
                            address.Country == Country)
                            return true;
                        break;
                    }
            }

            return base.Equals(obj);
        }


        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), AddressLine1, AddressLine2, City, ZipCode, State, Country);
    }

}
