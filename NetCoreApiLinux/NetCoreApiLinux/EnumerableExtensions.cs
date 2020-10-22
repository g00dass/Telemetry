using System;
using System.Collections.Generic;

namespace NetCoreApiLinux.Controllers
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Do<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var x in collection)
            {
                action(x);
                yield return x;
            }
        }
    }
}
