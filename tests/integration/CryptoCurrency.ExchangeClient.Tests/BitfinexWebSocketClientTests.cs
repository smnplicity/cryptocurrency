using System.Threading.Tasks;

using NUnit.Framework;

using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.ExchangeClient.Tests
{
    [TestFixture]
    public class BitfinexWebSocketClientTests
    {
        private ICurrencyFactory CurrencyFactory { get; set; }

        private ISymbolFactory SymbolFactory { get; set; }

        private IExchange Exchange { get; set; }

        private ExchangeWebSocketClientTests WebSocketTest { get; set; }

        [SetUp]
        protected async Task SetUp()
        {
            CurrencyFactory = CommonMock.GetCurrencyFactory();
            SymbolFactory = CommonMock.GetSymbolFactory();

            Exchange = new Bitfinex.Bitfinex(CurrencyFactory, SymbolFactory);

            await Exchange.Initialize();

            WebSocketTest = new ExchangeWebSocketClientTests(Exchange);
        }

        [Test]
        public void CanReceiveTrades()
        {
            var symbol = SymbolFactory.Get(CurrencyCodeEnum.ETH, CurrencyCodeEnum.BTC);

            WebSocketTest.CanReceiveTrades(symbol);
        }
    }
}
