﻿using System.Threading.Tasks;

using NUnit.Framework;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.Currency;

namespace CryptoCurrency.ExchangeClient.Tests
{
    [TestFixture]
    public class BitfinexHttpClientTests
    {
        private ICurrencyFactory CurrencyFactory { get; set; }

        private ISymbolFactory SymbolFactory { get; set; }

        private IExchange Exchange { get; set; }

        [SetUp]
        protected async Task SetUp()
        {
            CurrencyFactory = CommonMock.GetCurrencyFactory();
            SymbolFactory = CommonMock.GetSymbolFactory();

            Exchange = new Bitfinex.Bitfinex(CurrencyFactory, SymbolFactory);

            await Exchange.Initialize();
        }

        [Test]
        public async Task GetTradesRequestIsValid()
        {
            foreach (var symbolCode in Exchange.Symbol)
            {
                var symbol = SymbolFactory.Get(symbolCode);

                await ExchangeHttpClientTests.HttpGetTradeRequestIsValid(Exchange, symbol);
            }
        }
    }
}
