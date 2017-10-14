using System.Collections.Generic;
using System.Linq;

namespace Asvarduil.Penumbra.Services.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                return true;

            if (collection is ICollection<T>)
            {
                ICollection<T> list = collection as ICollection<T>;
                return list.Count == 0;
            }

            return collection.Count() == 0;
        }
    }
}
