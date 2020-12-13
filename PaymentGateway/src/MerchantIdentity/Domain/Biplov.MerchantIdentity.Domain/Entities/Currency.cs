using System.Collections.Generic;
using Biplov.Common.Core;

namespace Biplov.MerchantIdentity.Domain.Entities
{
    public class Currency : ValueObject
    {
        public string IsoCode { get; }

        public Currency(string isoCode)
        {
            IsoCode = isoCode;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return IsoCode;
        }
    }
}
