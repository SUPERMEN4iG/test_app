using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Extensions
{
    public static class IEnumerableExtensions
    {
        public static T RandomElementByWeight<T>(this IEnumerable<T> sequence, Func<T, float> weightSelector)
        {
            float totalWeight = sequence.Sum(weightSelector);
            double itemWeightIndex = new Random().NextDouble() * totalWeight;
            float currentWeightIndex = 0;

            foreach (var item in sequence.Select(x => new { Value = x, Weight = weightSelector(x) }))
            {
                currentWeightIndex += item.Weight;

                if (currentWeightIndex >= itemWeightIndex)
                    return item.Value;
            }

            return default(T);

        }
    }
}
