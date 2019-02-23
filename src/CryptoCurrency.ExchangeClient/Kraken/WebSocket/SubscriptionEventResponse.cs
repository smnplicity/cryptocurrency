using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Kraken.WebSocket
{
    public class SubscriptionEventResponse : BaseEvent
    {
        [JsonProperty("channelID")]
        public int ChannelId { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("pair")]
        public string Pair { get; set; }
    }
}
