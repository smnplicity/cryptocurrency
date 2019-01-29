using System;

using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.CoinbasePro.Model
{
    public class CoinbaseProTick
    {
        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "bid")]
        public double Bid { get; set; }

        [JsonProperty(PropertyName = "ask")]
        public double Ask { get; set; }

        [JsonProperty(PropertyName = "volume")]
        public double Volume { get; set; }

        [JsonProperty(PropertyName = "time")]
        public DateTime Time { get; set; }
    }
}
