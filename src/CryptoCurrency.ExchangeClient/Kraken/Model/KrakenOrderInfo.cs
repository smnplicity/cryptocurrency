using System.Collections.Generic;

using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Kraken.Model
{
    public class KrakenOpenOrders
    {
        [JsonProperty(PropertyName = "open")]
        public Dictionary<string, KrakenOrderInfo> Open { get; set; }
    }

    public class KrakenClosedOrders
    {
        [JsonProperty(PropertyName = "closed")]
        public Dictionary<string, KrakenOrderInfo> Closed { get; set; }
    }

    public class KrakenOrderQuery : Dictionary<string, KrakenOrderInfo>
    {

    }

    public class KrakenOrderDescr
    {
        [JsonProperty(PropertyName = "pair")]
        public string Pair { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "ordertype")]
        public string OrderType { get; set; }
    }

    public class KrakenOrderInfo
    {
        [JsonProperty(PropertyName = "refid")]
        public string RefId { get; set; }

        [JsonProperty(PropertyName = "userref")]
        public string UserRef { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "descr")]
        public KrakenOrderDescr Descr { get; set; }

        [JsonProperty(PropertyName = "opentm")]
        public double OpenTm { get; set; }

        [JsonProperty(PropertyName = "vol")]
        public double Volume { get; set; }

        [JsonProperty(PropertyName = "vol_exec")]
        public double VolumeExec { get; set; }

        [JsonProperty(PropertyName = "cost")]
        public double Cost { get; set; }

        [JsonProperty(PropertyName = "fee")]
        public double Fee { get; set; }

        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "limitprice")]
        public double LimitPrice { get; set; }
    }
}
