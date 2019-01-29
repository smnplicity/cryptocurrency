using System;
using System.Collections.Generic;


using NUnit.Framework;

using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.Currency;

namespace CryptoCurrency.Core.Tests
{
    [TestFixture]
    public class SymbolFactoryTests
    {
        private ICurrencyFactory CurrencyFactory { get; set; }

        private ISymbolFactory SymbolFactory { get; set; }

        [SetUp]
        protected void SetUp()
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

            CurrencyFactory = new CurrencyFactory(currency);
            SymbolFactory = new SymbolFactory(CurrencyFactory);
        }

        [Test]
        public void ContainsAllSymbols()
        {
            var symbolCount = Enum.GetValues(typeof(SymbolCodeEnum)).Length;
            var resolvedSymbols = SymbolFactory.List().Count;

            Assert.AreEqual(symbolCount, resolvedSymbols);
        }

        [Test]
        public void ResolvesTradableBtc()
        {
            var symbol = SymbolFactory.Get(CurrencyCodeEnum.BTC, CurrencyCodeEnum.USD);

            Assert.IsTrue(symbol.Code == SymbolCodeEnum.BTCUSD);
            Assert.IsTrue(symbol.Tradable);
        }
    }
}
