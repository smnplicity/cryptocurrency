using System;
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
            CurrencyFactory = CommonMock.GetCurrencyFactory();
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
