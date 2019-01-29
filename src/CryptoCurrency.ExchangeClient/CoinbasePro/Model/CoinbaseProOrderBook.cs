using System.Collections.Generic;

using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.CoinbasePro.Model
{
    public class CoinbaseProOrderBook
    {
        [JsonProperty(PropertyName = "bids")]
        public ICollection<ICollection<double>> Bids { get; set; }

        [JsonProperty(PropertyName = "asks")]
        public ICollection<ICollection<double>> Asks { get; set; }
    }
}
