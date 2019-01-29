using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoCurrency.Core.Extensions
{
    public static class MathExtensions
    {
        public static double Variance(this IEnumerable<double> values)
        {
            var avg = values.Average();

            var nominator = values.Sum(v => Math.Pow(v - avg, 2));

            return nominator / values.Count();
        }

        public static double StandardDeviation(this IEnumerable<double> values)
        {
            var M = 0.0;
            var S = 0.0;
            var k = 1;

            foreach (double value in values)
            {
                var tmpM = M;
                M += (value - tmpM) / k;
                S += (value - tmpM) * (value - M);
                k++;
            }

            return Math.Sqrt(S / (k - 2));
        }
    }
}
