using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Helper
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> GroupSimple<T>(
       this IEnumerable<T> source, Func<T, T, bool> predicate)
        {
            using (var iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                    yield break;

                List<T> list = new List<T>() { iterator.Current };

                T previous = iterator.Current;

                while (iterator.MoveNext())
                {
                    if (predicate(previous, iterator.Current))
                    {
                        list.Add(iterator.Current);
                    }
                    else
                    {
                        yield return list;
                        list = new List<T>() { iterator.Current };
                    }

                    previous = iterator.Current;
                }
                yield return list;
            }
        }
    }
}
