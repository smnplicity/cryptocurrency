using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Binance.Model
{
    public class BinanceOrderBookTicker
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("bidPrice")]
        public decimal BidPrice { get; set; }

        [JsonProperty("bidQty")]
        public decimal BidQty { get; set; }

        [JsonProperty("askPrice")]
        public decimal AskPrice { get; set; }

        [JsonProperty("askQty")]
        public decimal AskQty { get; set; }
    }
}
