using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Binance.Model
{
    public class BinanceOrderBookTicker
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("bidPrice")]
        public double BidPrice { get; set; }

        [JsonProperty("bidQty")]
        public double BidQty { get; set; }

        [JsonProperty("askPrice")]
        public double AskPrice { get; set; }

        [JsonProperty("askQty")]
        public double AskQty { get; set; }
    }
}
