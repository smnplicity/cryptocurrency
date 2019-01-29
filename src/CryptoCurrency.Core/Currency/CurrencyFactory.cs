using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoCurrency.Core.Currency
{
    public class CurrencyFactory : ICurrencyFactory
    {
        private ICollection<ICurrency> Currency { get; set; }

        public CurrencyFactory(IEnumerable<ICurrency> currency)
        {
            Currency = currency.ToList();
        }

        public ICurrency Get(CurrencyCodeEnum code)
        {
            var matched = Currency.Where(c => c.Code == code).FirstOrDefault();

            if (matched == null)
                throw new ArgumentException($"No currency could be found for code '{code}'");

            return matched;
        }

        public ICurrency Get(string code)
        {
            if (!Enum.TryParse(code, out CurrencyCodeEnum currencyCode))
                throw new ArgumentException($"'{code}' is not a valid currency code");

            return Get(currencyCode);
        }

        public ICollection<ICurrency> List()
        {
            return Currency;
        }
    }
}
