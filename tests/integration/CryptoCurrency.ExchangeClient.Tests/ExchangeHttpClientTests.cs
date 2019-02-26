using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.ExchangeClient.Tests
{
    public class ExchangeHttpClientTests
    {
        [TestMethod]
        public static async Task HttpGetTradeRequestIsValid(IExchange exchange, ISymbol symbol)
        {
            var httpClient = exchange.GetHttpClient();

            if (httpClient == null)
                Assert.Fail($"HTTP client implementation missing for {exchange.Name}");

            var result = await httpClient.GetTrades(symbol, 1, null);

            Assert.IsTrue(result.StatusCode == WrappedResponseStatusCode.Ok, $"Returned status {result.StatusCode}");
            Assert.IsTrue(result.Data.Exchange == exchange.Name);
            Assert.IsTrue(result.Data.SymbolCode == symbol.Code);

            if(result.Data.Trades.Count > 0)
            {
                var firstTrade = result.Data.Trades.First();

                Assert.IsTrue(firstTrade.Exchange == exchange.Name);
                Assert.IsTrue(firstTrade.SymbolCode == symbol.Code);
                Assert.IsFalse(firstTrade.Price.Equals(0.0));
                Assert.IsFalse(firstTrade.Volume.Equals(0.0));
            }
        }
    }
}
