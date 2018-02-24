using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Extensions
{
    public static class IEnumerableExtensions


    {
        private static Random _random = new Random();
        public static T RandomElementByWeight<T>(this IEnumerable<T> sequence, Func<T, float> weightSelector)
        {
            float totalWeight = sequence.Sum(weightSelector);
            double itemWeightIndex = GetRandomNumber(1, totalWeight);

            foreach (var item in sequence.Select(x => new { Value = x, Weight = weightSelector(x) }))
            {
                itemWeightIndex -= item.Weight;

                if (itemWeightIndex <= 0)
                    return item.Value;
            }

            return default(T);

        }

        private static double GetRandomNumber(double minimum, double maximum)
        {

            return _random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}