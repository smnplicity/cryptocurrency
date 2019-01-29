using System.Collections.Generic;

using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Kraken.Model
{
    public class KrakenPairTick
    {
        [JsonProperty(PropertyName = "a")]
        public ICollection<double> Ask { get; set; }

        [JsonProperty(PropertyName = "b")]
        public ICollection<double> Bid { get; set; }

        [JsonProperty(PropertyName = "c")]
        public ICollection<double> Last { get; set; }
    }
    
    public class KrakenTick : Dictionary<string, KrakenPairTick>
    {

    }


    public class KrakenTicks : Dictionary<string, KrakenPairTick>
    {

    }
}
