using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.shared.Extensions
{
    public static class IEnumerableExtensions
    {
        public static T RandomElementByWeight<T>(this IEnumerable<T> sequence, Func<T, float> weightSelector)
        {
            float totalWeight = sequence.Sum(weightSelector);
            double itemWeightIndex = new Random().NextDouble() * totalWeight;


            foreach (var item in sequence.Select(x => new { Value = x, Weight = weightSelector(x) }))
            {
                itemWeightIndex -= item.Weight;

                if (itemWeightIndex <= 0)
                    return item.Value;
            }

            return default(T);

        }
    }


}
