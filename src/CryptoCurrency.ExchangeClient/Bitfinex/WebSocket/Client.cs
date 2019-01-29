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

        public void SetApiAccess(string privateKey, string publicKey, string passphrase)
        {

        }
        
        public Task Begin()
        {
            return Task.Run(() =>
            {
                if (WebSocketClient != null)
                    throw new Exception("WebSocket already in use");

                Channels = new Dictionary<long, SubscriptionEventResponse>();

                using (WebSocketClient = new WebSocketSharp.WebSocket(Url))
                {
                    WebSocketClient.OnOpen += OnOpen;

                    WebSocketClient.OnMessage += OnMessage;

                    WebSocketClient.OnClose += delegate(object sender, WebSocketSharp.CloseEventArgs e)
                    {
                        Channels = new Dictionary<long, SubscriptionEventResponse>();
                        
                        OnClose?.Invoke(sender, new CloseEventArgs { });
                    };

                    Connect();

                    Task.Run(() =>
                    {
                        while (true)
                        {
                            WebSocketClient.Ping();

                            Task.Delay(15000);
                        }
                    });
                }
            });
        }

        public void Connect()
        {
            if(WebSocketClient != null)
                WebSocketClient.Connect();
        }

        public void BeginListenTrades(ISymbol symbol)
        {
            WebSocketClient.Send(JsonConvert.SerializeObject(new TradeRequest { Event = "subscribe", Channel = "trades", Symbol = $"t{symbol.Code.ToString()}" }));
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
                                    var trades = JsonConvert.DeserializeObject<TradeEventResponse>(message);

                                    Channels.Add(trades.ChanId, trades);

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

            OnTradesReceived(this, new TradesReceivedEventArgs { Data = result });
        }
        #endregion
    }
}
