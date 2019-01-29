using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Binance.Model
{
    public class BinanceOrderBookTicker
    {
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "bidPrice")]
        public double BidPrice { get; set; }

        [JsonProperty(PropertyName = "bidQty")]
        public double BidQty { get; set; }

        [JsonProperty(PropertyName = "askPrice")]
        public double AskPrice { get; set; }

        [JsonProperty(PropertyName = "askQty")]
        public double AskQty { get; set; }
    }
}
