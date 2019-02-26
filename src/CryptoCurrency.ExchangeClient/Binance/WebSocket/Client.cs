using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using WebSocketSharp;

using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Extensions;
using CryptoCurrency.Core.Symbol;

using CloseEventArgs = CryptoCurrency.Core.Exchange.CloseEventArgs;

namespace CryptoCurrency.ExchangeClient.Binance.WebSocket
{
    public class Client : IExchangeWebSocketClient
    {
        private Binance Exchange { get; set; }
        private ICurrencyFactory CurrencyFactory { get; set; }
        private ISymbolFactory SymbolFactory { get; set; }
        private WebSocketSharp.WebSocket WebSocketClient { get; set; }
        private HashSet<string> Streams { get; set; }

        public Client(Binance ex, ICurrencyFactory currencyFactory, ISymbolFactory symbolFactory)
        {
            Exchange = ex;
            CurrencyFactory = currencyFactory;
            SymbolFactory = symbolFactory;
        }

        public string Url => "wss://stream.binance.com:9443";

        public bool IsSubscribeModel => false;

        public event EventHandler OnOpen;
        public event EventHandler<CloseEventArgs> OnClose;
        public event EventHandler<TradesReceivedEventArgs> OnTradesReceived;
        public event EventHandler<TickerReceivedEventArgs> OnTickerReceived;
        
        private void BeginWebSocketClient(string stream, ICollection<ISymbol> symbols)
        {
            var newStreams = symbols.Select(s => $"{(Exchange.GetCurrencyCode(s.BaseCurrencyCode) + Exchange.GetCurrencyCode(s.QuoteCurrencyCode)).ToLower()}@{stream}");

            Streams.UnionWith(newStreams);

            if (WebSocketClient != null)
                WebSocketClient = null;
            
            WebSocketClient = new WebSocketSharp.WebSocket($"{Url}/stream?streams={string.Join('/', Streams)}");
            
            WebSocketClient.OnOpen += OnOpen;

            WebSocketClient.OnMessage += OnMessage;

            WebSocketClient.OnClose += delegate (object sender, WebSocketSharp.CloseEventArgs e)
            {
                OnClose?.Invoke(sender, new CloseEventArgs
                {
                    Code = e.Code,
                    Reason = e.Reason
                });
            };

            WebSocketClient.Connect();
        }

        public Task Begin() => Task.Run(() =>
        {
            Streams = new HashSet<string>();
        });

        public void BeginListenTicker(ICollection<ISymbol> symbols)
        {
            BeginWebSocketClient("ticker", symbols);
        }

        public void BeginListenTrades(ICollection<ISymbol> symbols)
        {
            BeginWebSocketClient("aggTrade", symbols);
        }

        public void Connect()
        {
            if (WebSocketClient != null)
                WebSocketClient.Connect();
        }

        public void SetApiAccess(string privateKey, string publicKey, string passphrase)
        {
            throw new NotImplementedException();
        }

        private async void OnMessage(object sender, MessageEventArgs e)
        {
            var message = JsonConvert.DeserializeObject<BaseMessage>(e.Data);

            var symbol = message.Stream.Split("@")[0].ToUpper();
            var channel = message.Stream.Split("@")[1];

            switch (channel)
            {
                case "aggTrade":
                    var tradeAggMessage = JsonConvert.DeserializeObject<Message<Dictionary<string, object>>>(e.Data);

                    var postData = new NameValueCollection()
                    {
                        { "symbol", symbol }
                    };

                    OnTradesReceived?.Invoke(null, new TradesReceivedEventArgs
                    {
                        Data = await Exchange.ChangeType<ICollection<Dictionary<string, object>>, TradeResult>(CurrencyFactory, SymbolFactory, postData, new[] { tradeAggMessage.Data })
                    });

                    break;
            }
        }
    }
}
