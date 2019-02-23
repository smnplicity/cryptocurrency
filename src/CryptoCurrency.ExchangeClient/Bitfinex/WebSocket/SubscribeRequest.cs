using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Bitfinex.WebSocket
{
    public class SubscribeRequest
    {
        [JsonProperty(PropertyName = "event")]
        public string Event { get; set; }

        [JsonProperty(PropertyName = "channel")]
        public string Channel { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }
    }
}