using System;
using System.Collections.Generic;

using NUnit.Framework;

using CryptoCurrency.Core.Currency;

namespace CryptoCurrency.Core.Tests
{
    [TestFixture]
    public class CurrencyFactoryTests
    {
        private ICurrencyFactory CurrencyFactory { get; set; }
        
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
        }

        [Test]
        public void ContainsAllCurrencies()
        {
            var currencyCount = Enum.GetValues(typeof(CurrencyCodeEnum)).Length;
            var resolvedCurrencies = CurrencyFactory.List().Count;

            Assert.AreEqual(currencyCount, resolvedCurrencies);
        }
    }
}
