using System;

namespace CryptoCurrency.Core.Extensions
{
    public static class IntExtensions
    {
        public static string PadLeft(this int value, int length)
        {
            var padding = length - Convert.ToString(value).Length;

            var ret = "";

            for (var i = 0; i < padding; i++)
                ret += "0";

            return $"{ret}{value}";
        }
    }
}
