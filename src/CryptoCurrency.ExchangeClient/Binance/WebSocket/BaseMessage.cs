using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Binance.WebSocket
{
    public class BaseMessage
    {
        [JsonProperty("stream")]
        public string Stream { get; set; }
    }

}
