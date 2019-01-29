using System.Collections.Generic;

using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Kraken.Model
{
    public class KrakenTradeFee
    {
        [JsonProperty(PropertyName = "fee")]
        public double Fee { get; set; }
    }

    public class KrakenTradeVolume
    {
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "volume")]
        public double Volume { get; set; }

        [JsonProperty(PropertyName = "fees")]
        public Dictionary<string, KrakenTradeFee> Fees { get; set; }

        [JsonProperty(PropertyName = "fees_maker")]
        public Dictionary<string, KrakenTradeFee> FeesMaker { get; set; }
    }
}
