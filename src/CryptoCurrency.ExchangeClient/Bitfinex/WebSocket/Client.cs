using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharp;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.Exchange.Model;

using CloseEventArgs = CryptoCurrency.Core.Exchange.CloseEventArgs;
using CryptoCurrency.Core.Market;

namespace CryptoCurrency.ExchangeClient.Bitfinex.WebSocket
{
    public class Client : IExchangeWebSocketClient
    {
        private Bitfinex Exchange { get; set; }

        private ISymbolFactory SymbolFactory { get; set; }

        public Client(Bitfinex ex, ISymbolFactory symbolFactory)
        {
            Exchange = ex;

            SymbolFactory = symbolFactory;
        }

        public string Url => "wss://api.bitfinex.com/ws/2";

        private WebSocketSharp.WebSocket WebSocketClient { get; set; }

        private Dictionary<long, SubscriptionEventResponse> Channels { get; set; }

        public event EventHandler OnOpen;

        public event EventHandler<CloseEventArgs> OnClose;

        public event EventHandler<TradesReceivedEventArgs> OnTradesReceived;

        public event EventHandler<TickerReceivedEventArgs> OnTickerReceived;

        public void SetApiAccess(string privateKey, string publicKey, string passphrase)
        {

        }

        public Task Begin() => Task.Run(() =>
        {
            if (WebSocketClient != null)
                throw new Exception("WebSocket already in use");

            Channels = new Dictionary<long, SubscriptionEventResponse>();

            using (WebSocketClient = new WebSocketSharp.WebSocket(Url))
            {
                WebSocketClient.OnOpen += OnOpen;

                WebSocketClient.OnMessage += OnMessage;

                WebSocketClient.OnClose += delegate (object sender, WebSocketSharp.CloseEventArgs e)
                {
                    Channels = new Dictionary<long, SubscriptionEventResponse>();

                    OnClose?.Invoke(sender, new CloseEventArgs { });
                };

                Connect();
            }
        });

        public void Connect()
        {
            if(WebSocketClient != null)
                WebSocketClient.Connect();
        }

        public void BeginListenTrades(ISymbol symbol)
        {
            WebSocketClient.Send(JsonConvert.SerializeObject(new SubscribeRequest { Event = "subscribe", Channel = "trades", Symbol = $"t{symbol.Code.ToString()}" }));
        }

        public void BeginListenTicker(ISymbol symbol)
        {
            WebSocketClient.Send(JsonConvert.SerializeObject(new SubscribeRequest { Event = "subscribe", Channel = "ticker", Symbol = $"t{symbol.Code.ToString()}" }));
        }

        #region Private functionality
        private void OnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Data.Trim();

            if (!string.IsNullOrEmpty(message))
            {
                if (message[0] == '{')
                {
                    var evt = JsonConvert.DeserializeObject<BaseEventResponse>(message);

                    if (evt == null)
                        return;

                    switch (evt.Event)
                    {
                        case "subscribed":
                            var subscription = JsonConvert.DeserializeObject<SubscriptionEventResponse>(message);

                            switch (subscription.Channel)
                            {
                                case "trades":
                                    var tradeResponse = JsonConvert.DeserializeObject<TradeEventResponse>(message);

                                    Channels.Add(tradeResponse.ChanId, tradeResponse);

                                    break;
                                case "ticker":
                                    var tickerResponse = JsonConvert.DeserializeObject<TickerEventResponse>(message);

                                    Channels.Add(tickerResponse.ChanId, tickerResponse);

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

                        switch (channel.Channel)
                        {
                            case "trades":
                                HandleTradeMessage((TradeEventResponse)channel, data);

                                break;
                            case "ticker":
                                HandleTickerMessage((TickerEventResponse)channel, data);

                                break;
                        }

                    }
                }
            }
        }

        private void HandleTradeMessage(TradeEventResponse channel, dynamic data)
        {
            var raw = new JArray();

            if (data[1] is JArray)
            {
                raw = data[1];
            }
            else
            {
                var type = Convert.ToString(data[1]);

                if (type != "tu")
                    return;

                raw.Add(data[2]);
            }

            var pair = Exchange.DecodeSymbol(channel.Symbol);

            var symbol = SymbolFactory.Get(pair[0], pair[1]);

            var additionalData = new NameValueCollection();
            additionalData.Add("SymbolCode", symbol.Code.ToString());

            var result = Exchange.ChangeType<dynamic, TradeResult>(SymbolFactory, raw, null, additionalData);

            OnTradesReceived?.Invoke(this, new TradesReceivedEventArgs { Data = result });
        }

        private void HandleTickerMessage(TickerEventResponse channel, dynamic data)
        {
            if (data[1].GetType() != typeof(JArray))
                return;

            var raw = (data[1] as JArray).ToObject<object[]>();

            var pair = Exchange.DecodeSymbol(channel.Symbol);

            var symbol = SymbolFactory.Get(pair[0], pair[1]);

            var additionalData = new NameValueCollection();
            additionalData.Add("SymbolCode", symbol.Code.ToString());

            var result = Exchange.ChangeType<dynamic, MarketTick>(SymbolFactory, raw, null, additionalData);

            OnTickerReceived?.Invoke(this, new TickerReceivedEventArgs { Data = result });
        }
        #endregion
    }
}
