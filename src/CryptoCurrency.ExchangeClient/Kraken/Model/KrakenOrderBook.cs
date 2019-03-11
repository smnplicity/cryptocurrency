using System.Collections.Generic;

using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Kraken.Model
{
    public class KrakenOrderBookPair
    {
        [JsonProperty(PropertyName = "asks")]
        public ICollection<ICollection<decimal>> Asks { get; set; }

        [JsonProperty(PropertyName = "bids")]
        public ICollection<ICollection<decimal>> Bids { get; set; }
    }

    public class KrakenOrderBook : Dictionary<string, KrakenOrderBookPair>
    {

    }
}
