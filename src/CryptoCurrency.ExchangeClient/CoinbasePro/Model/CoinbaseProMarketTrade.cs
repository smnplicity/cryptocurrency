using System;

using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.CoinbasePro.Model
{
    public class CoinbaseProMarketTrade
    {
        [JsonProperty(PropertyName = "time")]
        public DateTime Time { get; set; }

        [JsonProperty(PropertyName = "trade_id")]
        public long TradeId { get; set; }

        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "size")]
        public double Size { get; set; }

        [JsonProperty(PropertyName = "side")]
        public string Side { get; set; }
    }
}
