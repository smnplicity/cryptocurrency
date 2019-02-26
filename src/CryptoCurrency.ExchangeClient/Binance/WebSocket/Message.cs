using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Binance.WebSocket
{
    public class Message<T> : BaseMessage
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
