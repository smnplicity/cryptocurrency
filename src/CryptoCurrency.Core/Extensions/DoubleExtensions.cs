using System;

namespace CryptoCurrency.Core.Extensions
{
    public static class DoubleExtensions
    {
        public static double RoundDown(this double value, int precision)
        {
            var power = Math.Pow(10, precision);

            return Math.Floor(value * power) / power;
        }

        public static double Round(this double value, int precision)
        {
            return Math.Round(value, precision);
        }
    }
}
