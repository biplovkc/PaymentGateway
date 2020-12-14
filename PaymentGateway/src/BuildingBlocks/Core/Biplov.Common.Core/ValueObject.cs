using System.Collections.Generic;
using System.Linq;

namespace Biplov.Common.Core
{
    /// <summary>
    /// Any class that inherit ValueObject should be a simple entity.
    /// i.e. They don't need to have their own unique identity,
    /// and is owned by other Entities. Possibly aggregate roots.
    /// </summary>
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (left is null ^ right is null)
                return false;

            return left is null || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right) => !EqualOperator(left, right);

        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object obj)
        {
            if (obj is null || obj.GetType() != GetType())
                return false;

            var other = (ValueObject) obj;
            using var thisValues = GetAtomicValues().GetEnumerator();
            using var otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (thisValues.Current is null ^ otherValues.Current is null)
                    return false;

                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                    return false;

            }

            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override int GetHashCode() =>
            GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);

        public ValueObject GetCopy() => MemberwiseClone() as ValueObject;
    }
}
