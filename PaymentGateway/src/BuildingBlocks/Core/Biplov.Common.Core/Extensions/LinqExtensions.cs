using System.Collections.Generic;
using System.Linq;

namespace Biplov.Common.Core.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Checks if any given sequence is empty or not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> sequence)
        {
            return sequence ?? Enumerable.Empty<T>();
        }
    }
}
