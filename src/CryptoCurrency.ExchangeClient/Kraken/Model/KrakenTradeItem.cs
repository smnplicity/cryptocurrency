using System.Collections.Generic;

using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Kraken.Model
{
    public class KrakenTradeHistory
    {
        [JsonProperty(PropertyName = "trades")]
        public Dictionary<string, KrakenTradeItem> Trades { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
    }

    public class KrakenTradeItem
    {
        [JsonProperty(PropertyName = "ordertxid")]
        public string OrderTxId { get; set; }

        [JsonProperty(PropertyName = "pair")]
        public string Pair { get; set; }

        [JsonProperty(PropertyName = "time")]
        public long Time { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "ordertype")]
        public string OrderType { get; set; }

        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "cost")]
        public double Cost { get; set; }

        [JsonProperty(PropertyName = "fee")]
        public double Fee { get; set; }

        [JsonProperty(PropertyName = "vol")]
        public double Volume { get; set; }
    }
}
