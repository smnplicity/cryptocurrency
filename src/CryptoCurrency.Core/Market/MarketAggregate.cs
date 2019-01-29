using Newtonsoft.Json;

namespace CryptoCurrency.Core.Market
{
    public class MarketAggregate : Ohlc
    {
        [JsonProperty("buyVolume")]
        public double? BuyVolume { get; set; }

        [JsonProperty("sellVolume")]
        public double? SellVolume { get; set; }

        [JsonProperty("totalVolume")]
        public double? TotalVolume { get; set; }

        [JsonProperty("buyCount")]
        public int? BuyCount { get; set; }

        [JsonProperty("sellCount")]
        public int? SellCount { get; set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}
