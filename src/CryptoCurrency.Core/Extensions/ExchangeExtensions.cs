using System;
using System.Linq;

using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Extensions
{
    public static class ExchangeExtensions
    {
        public static void EnsureSymbol(this IExchange ex, ISymbol symbol)
        {
            var match = ex.Symbol.Any(s => s == symbol.Code);

            if (!match)
                throw new ArgumentException($"Exchange {ex.Name} doesn't support {symbol.Code}");
        }

        public static string GetCurrencyCode(this IExchange ex, CurrencyCodeEnum currencyCode)
        {
            var currency = ex.Currency.Where(c => c.CurrencyCode == currencyCode).FirstOrDefault();

            if (currency == null)
                throw new Exception($"Currency code {currencyCode} not supported by exchange {ex.Name}");

            return currency.AltCurrencyCode ?? currency.CurrencyCode.ToString();
        }

        public static CurrencyCodeEnum GetStandardisedCurrencyCode(this IExchange ex, string currencyCode)
        {
            var currency = ex.Currency.Where(c => (string.IsNullOrEmpty(c.AltCurrencyCode) && c.CurrencyCode.ToString() == currencyCode) || c.AltCurrencyCode == currencyCode).FirstOrDefault();

            if (currency == null)
                throw new Exception($"Currency code {currencyCode} not supported by exchange {ex.Name}");

            return currency.CurrencyCode;
        }
    }
}