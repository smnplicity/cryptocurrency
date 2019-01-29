using System;

using CryptoCurrency.Core.Exchange;

namespace CryptoCurrency.Core.Extensions
{
    public static class ExchangeHttpClientExtensions
    {
        public static string GetFullUrl(this IExchangeHttpClient ex, string relativeUrl)
        {
            return new Uri(new Uri(ex.ApiUrl), relativeUrl).ToString();
        }
    }
}
