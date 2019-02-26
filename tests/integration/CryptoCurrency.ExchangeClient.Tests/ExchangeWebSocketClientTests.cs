using System;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.Currency;

namespace CryptoCurrency.ExchangeClient.Tests
{
    public class ExchangeWebSocketClientTests
    {
        private ISymbolFactory SymbolFactory { get; set; }

        private IExchange Exchange { get; set; }

        public ExchangeWebSocketClientTests(IExchange exchange)
        {
            Exchange = exchange;

            SymbolFactory = CommonMock.GetSymbolFactory();
        }

        [TestMethod]
        public void CanConnect()
        {
            var resetEvent = new ManualResetEvent(false);

            var webSocketClient = Exchange.GetWebSocketClient();

            if (webSocketClient == null)
            {
                Assert.Fail($"Web Socket client not available for {Exchange.Name}");

                return;
            }

            var retry = 0;

            webSocketClient.OnOpen += delegate (object sender, EventArgs e)
            {
                resetEvent.Set();
            };

            webSocketClient.OnClose += delegate (object sender, CloseEventArgs e)
            {
                if(retry >= 3)
                {
                    Assert.Fail($"Unable to connect to web socket client after {retry} attempts");

                    resetEvent.Set();
                }
                else
                {
                    retry++;

                    webSocketClient.Connect();
                }
            };

            webSocketClient.Begin();

            resetEvent.WaitOne(3000);
        }

        [TestMethod]
        public void CanReceiveTrades(ISymbol symbol)
        {
            var resetEvent = new ManualResetEvent(false);

            var webSocketClient = Exchange.GetWebSocketClient();
            
            webSocketClient.OnOpen += delegate (object sender, EventArgs e)
            {
                if (webSocketClient.IsSubscribeModel)
                    webSocketClient.BeginListenTrades(new[] { symbol });
            };

            var retry = 0;

            webSocketClient.OnClose += delegate (object sender, CloseEventArgs e)
            {
                if (retry >= 3)
                {
                    Assert.Fail($"Unable to connect to web socket client after {retry} attempts");

                    resetEvent.Set();
                }
                else
                {
                    retry++;

                    webSocketClient.Connect();
                }
            };

            webSocketClient.OnTradesReceived += delegate (object sender, TradesReceivedEventArgs e)
            {
                webSocketClient = null;

                resetEvent.Set();
            };

            webSocketClient.Begin();

            if (!webSocketClient.IsSubscribeModel)
                webSocketClient.BeginListenTrades(new[] { symbol });

            resetEvent.WaitOne();
        }
    }
}
