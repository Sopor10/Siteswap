using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class IEnumerableExtensions
    {
        public static bool ContainsAllItemsFrom<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            return !b.Except(a).Any();
        }

        public static Maybe<T> SingleOrNone<T>(this IEnumerable<T> sequence, Func<T, bool> func)
        {
            var singleOrDefault = sequence.SingleOrDefault(func);
            if (singleOrDefault == null)
            {
                return new Maybe<T>();
            }
            return new Maybe<T>(singleOrDefault);
        }
        
        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> sequence, Func<T, bool> func)
        {
            var singleOrDefault = sequence.FirstOrDefault(func);
            if (singleOrDefault == null)
            {
                return new Maybe<T>();
            }
            return new Maybe<T>(singleOrDefault);
        }
    }
}