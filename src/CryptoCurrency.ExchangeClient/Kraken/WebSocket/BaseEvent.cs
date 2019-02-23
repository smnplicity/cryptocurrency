using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Kraken.WebSocket
{
    public class BaseEventInnerRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
    
    public class BaseEvent
    {
        [JsonProperty("event")]
        public string Event { get; set; }
        
        [JsonProperty("subscription")]
        public BaseEventInnerRequest Subscription { get; set; }
    }
}
