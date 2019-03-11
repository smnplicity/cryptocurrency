using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using WebSocketSharp;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.Extensions;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Market;
using CryptoCurrency.Core;
using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.Currency;

using CloseEventArgs = CryptoCurrency.Core.Exchange.CloseEventArgs;

namespace CryptoCurrency.ExchangeClient.Kraken.WebSocket
{
    public class Client : IExchangeWebSocketClient
    {
        private Kraken Exchange { get; set; }
        private ICurrencyFactory CurrencyFactory { get; set; }
        private ISymbolFactory SymbolFactory { get; set; }

        private WebSocketSharp.WebSocket WebSocketClient { get; set; }

        private Dictionary<long, SubscriptionEventResponse> Channels { get; set; }

        public Client(Kraken ex, ICurrencyFactory currencyFactory, ISymbolFactory symbolFactory)
        {
            Exchange = ex;
            CurrencyFactory = currencyFactory;
            SymbolFactory = symbolFactory;
        }

        public string Url => "wss://ws.kraken.com";

        public bool IsSubscribeModel => true;

        public event EventHandler OnOpen;
        public event EventHandler<CloseEventArgs> OnClose;
        public event EventHandler<TradesReceivedEventArgs> OnTradesReceived;
        public event EventHandler<TickerReceivedEventArgs> OnTickerReceived;

        public Task Begin() => Task.Run(() =>
        {
            if (WebSocketClient != null)
                throw new Exception("WebSocket already in use");

            Channels = new Dictionary<long, SubscriptionEventResponse>();

            WebSocketClient = new WebSocketSharp.WebSocket(Url);

            WebSocketClient.OnOpen += OnOpen;

            WebSocketClient.OnMessage += OnMessage;

            WebSocketClient.OnClose += delegate (object sender, WebSocketSharp.CloseEventArgs e)
            {
                Channels = new Dictionary<long, SubscriptionEventResponse>();

                OnClose?.Invoke(sender, new CloseEventArgs { });
            };

            Connect();
        });

        public void Connect()
        {
            if (WebSocketClient != null)
                WebSocketClient.Connect();
        }

        public void BeginListenTicker(ICollection<ISymbol> symbols)
        {
            throw new NotImplementedException();
        }

        public void BeginListenTrades(ICollection<ISymbol> symbols)
        {
            var pairs = new List<string>();

            foreach (var symbol in symbols)
            {
                var baseCurrency = Exchange.GetCurrencyCode(symbol.BaseCurrencyCode);
                var quoteCurrency = Exchange.GetCurrencyCode(symbol.QuoteCurrencyCode);

                pairs.Add($"{baseCurrency}/{quoteCurrency}");
            }

            WebSocketClient.Send(JsonConvert.SerializeObject(new SubscriptionRequest { Event = "subscribe", Pair = pairs, Subscription = new BaseEventInnerRequest { Name = "trade" } }));
        }
        
        public void SetApiAccess(string privateKey, string publicKey, string passphrase)
        {
            throw new NotImplementedException();
        }

        #region Private functionality
        private void OnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Data.Trim();

            if (!string.IsNullOrEmpty(message))
            {
                if (message[0] == '{')
                {
                    var evt = JsonConvert.DeserializeObject<BaseEvent>(message);

                    if (evt == null)
                        return;

                    switch (evt.Event)
                    {
                        case "subscriptionStatus":
                            var response = JsonConvert.DeserializeObject<SubscriptionEventResponse>(message);

                            switch(response.Status)
                            {
                                case "subscribed":
                                    switch (response.Subscription.Name)
                                    {
                                        case "trade":
                                            Channels.Add(response.ChannelId, response);

                                            break;
                                    }

                                    break;
                                case "unsubscribed":
                                    Channels.Remove(response.ChannelId);

                                    break;
                            }

                            break;
                    }
                }

                if (message[0] == '[')
                {
                    var data = JsonConvert.DeserializeObject<dynamic>(message);

                    if (data != null && data.Count > 0)
                    {
                        var channelId = (long)data[0];

                        var channel = Channels[channelId];

                        switch (channel.Subscription.Name)
                        {
                            case "trade":
                                HandleTradeMessage(channel, data);

                                break;
                        }

                    }
                }
            }
        }

        private void HandleTradeMessage(SubscriptionEventResponse channel, dynamic data)
        {
            if (!(data[1] is JArray))
                return;

            var raw = data[1] as JArray;

            var pair = channel.Pair.Split('/');

            var baseCurrencyCode = Exchange.GetStandardisedCurrencyCode(CurrencyFactory, pair[0]);
            var quoteCurrencyCode = Exchange.GetStandardisedCurrencyCode(CurrencyFactory, pair[1]);

            var symbol = SymbolFactory.Get(baseCurrencyCode, quoteCurrencyCode);

            var trades = raw.Select(t => new MarketTrade
            {
                Exchange = Exchange.Name,
                SymbolCode = symbol.Code,
                Epoch = Epoch.FromSeconds(Convert.ToDouble(t[2])),
                Price = Convert.ToDecimal(t[0]),
                Volume = Convert.ToDecimal(t[1]),
                Side = (string)t[3] == "b" ? OrderSideEnum.Buy : OrderSideEnum.Sell
            }).ToList();

            var tradeResult = new TradeResult
            {
                Exchange = Exchange.Name,
                SymbolCode = symbol.Code,
                Filter = null,
                Trades = trades
            };

            OnTradesReceived?.Invoke(this, new TradesReceivedEventArgs { Data = tradeResult });
        }
        #endregion
    }
}
