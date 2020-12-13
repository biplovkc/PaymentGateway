using System.Collections.Generic;
using Biplov.Common.Core;

namespace Biplov.PaymentGateway.Domain.Entities
{
    public class MetaData : ValueObject
    {
        private MetaData(){}
        public string Key { get; }
        public string Value { get; }

        public MetaData(string key, string value)
        {
            Key = key;
            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Key;
            yield return Value;
        }
    }
}
