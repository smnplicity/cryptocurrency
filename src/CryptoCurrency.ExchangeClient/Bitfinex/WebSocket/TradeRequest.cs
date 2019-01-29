using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Bitfinex.WebSocket
{
    public class TradeRequest
    {
        [JsonProperty(PropertyName = "event")]
        public string Event { get; set; }

        [JsonProperty(PropertyName = "channel")]
        public string Channel { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }
    }
}