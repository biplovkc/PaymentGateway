using System;
using System.Collections.Generic;
using Biplov.Common.Core;

namespace Biplov.PaymentGateway.Domain.Entities
{
    public class Notification : ValueObject
    {
        private Notification(){}
        public Uri SuccessUrl { get; }

        public Uri ErrorUrl { get; }

        public Notification(Uri successUrl, Uri errorUrl)
        {
            SuccessUrl = successUrl;
            ErrorUrl = errorUrl;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return SuccessUrl;
            yield return ErrorUrl;
        }
    }
}
