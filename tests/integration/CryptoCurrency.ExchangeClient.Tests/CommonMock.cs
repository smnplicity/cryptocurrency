using System.Collections.Generic;

using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.ExchangeClient.Tests
{
    public class CommonMock
    {
        public static ISymbolFactory GetSymbolFactory()
        {
            var currency = new List<ICurrency>();

            currency.Add(new Bitcoin());
            currency.Add(new Litecoin());
            currency.Add(new Ethereum());
            currency.Add(new EthereumClassic());
            currency.Add(new Ripple());
            currency.Add(new Aud());
            currency.Add(new Eur());
            currency.Add(new Usd());
            currency.Add(new Iota());
            currency.Add(new Neo());
            currency.Add(new Dash());
            currency.Add(new Tether());
            currency.Add(new StellarLumens());

            var currencyFactory = new CurrencyFactory(currency);

            return new SymbolFactory(currencyFactory);
        }
    }
}