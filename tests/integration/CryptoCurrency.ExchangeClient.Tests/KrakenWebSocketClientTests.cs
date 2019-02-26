using NUnit.Framework;

using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.ExchangeClient.Tests
{
    [TestFixture]
    public class KrakenWebSocketClientTests
    {
        private ICurrencyFactory CurrencyFactory { get; set; }

        private ISymbolFactory SymbolFactory { get; set; }

        private IExchange Exchange { get; set; }

        private ExchangeWebSocketClientTests WebSocketTest { get; set; }

        [SetUp]
        protected void SetUp()
        {
            CurrencyFactory = CommonMock.GetCurrencyFactory();
            SymbolFactory = CommonMock.GetSymbolFactory();

            Exchange = new Kraken.Kraken(CurrencyFactory, SymbolFactory);

            WebSocketTest = new ExchangeWebSocketClientTests(Exchange);
        }

        [Test]
        public void CanReceiveTrades()
        {
            var symbol = SymbolFactory.Get(CurrencyCodeEnum.BTC, CurrencyCodeEnum.USD);

            WebSocketTest.CanReceiveTrades(symbol);
        }
    }
}
