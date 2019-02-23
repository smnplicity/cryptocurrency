using Newtonsoft.Json;
using System.Collections.Generic;

namespace CryptoCurrency.ExchangeClient.Kraken.WebSocket
{
    public class SubscriptionRequest : BaseEvent
    {
        [JsonProperty("pair")]
        public ICollection<string> Pair { get; set; }
    }
}
