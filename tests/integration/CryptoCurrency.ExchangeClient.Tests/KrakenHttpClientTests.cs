using System.Threading.Tasks;

using NUnit.Framework;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.ExchangeClient.Tests
{
    [TestFixture]
    public class KrakenHttpClientTests
    {
        private ISymbolFactory SymbolFactory { get; set; }

        private IExchange Exchange { get; set; }

        [SetUp]
        protected void SetUp()
        {
            SymbolFactory = CommonMock.GetSymbolFactory();

            Exchange = new Kraken.Kraken(SymbolFactory);
        }

        [Test]
        public async Task GetTradesRequestIsValid()
        {
            foreach (var symbolCode in Exchange.Symbol)
            {
                var symbol = SymbolFactory.Get(symbolCode);

                await CommonExchangeClientTests.HttpGetTradeRequestIsValid(Exchange, symbol);
            }
        }
    }
}
