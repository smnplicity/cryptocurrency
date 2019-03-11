using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Binance.Model
{
    public class BinanceAggTrade
    {
        [JsonProperty(PropertyName = "a")]
        public long TradeId { get; set; }

        [JsonProperty(PropertyName = "p")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "q")]
        public decimal Volume { get; set; }

        [JsonProperty(PropertyName = "T")]
        public long Timestamp { get; set; }
    }
}
