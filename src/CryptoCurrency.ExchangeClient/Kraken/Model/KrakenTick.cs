using System.Collections.Generic;

using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Kraken.Model
{
    public class KrakenPairTick
    {
        [JsonProperty(PropertyName = "a")]
        public ICollection<decimal> Ask { get; set; }

        [JsonProperty(PropertyName = "b")]
        public ICollection<decimal> Bid { get; set; }

        [JsonProperty(PropertyName = "c")]
        public ICollection<decimal> Last { get; set; }
    }
    
    public class KrakenTick : Dictionary<string, KrakenPairTick>
    {

    }


    public class KrakenTicks : Dictionary<string, KrakenPairTick>
    {

    }
}
