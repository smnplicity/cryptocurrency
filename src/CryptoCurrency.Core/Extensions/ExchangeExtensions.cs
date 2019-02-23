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
            var currency = ex.CurrencyConverter.Where(c => c.CurrencyCode == currencyCode).FirstOrDefault();

            return currency != null && currency.AltCurrencyCode != null ? currency.AltCurrencyCode : currencyCode.ToString();
        }

        public static CurrencyCodeEnum GetStandardisedCurrencyCode(this IExchange ex, ICurrencyFactory currencyFactory, string currencyCode)
        {
            var currency = ex.CurrencyConverter.Where(c => c.AltCurrencyCode == currencyCode).FirstOrDefault();

            return currency != null ? currency.CurrencyCode : currencyFactory.Get(currencyCode).Code;
        }
    }
}