using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Binance.Model
{
    public class BinanceOpenOrder
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; set; }
        
        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("origQty")]
        public double OriginalQuantity { get; set; }

        [JsonProperty("executedQty")]
        public double ExecutedQuantity { get; set; }

        [JsonProperty("cummulativeQuoteQty")]
        public double CumulativeQuoteQuantity { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("timeInForce")]
        public string TimeInForce { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("side")]
        public string Side { get; set; }

        [JsonProperty("stopPrice")]
        public double StopPrice { get; set; }

        [JsonProperty("icebergQty")]
        public double IcebergQuantity { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("updateTime")]
        public long UpdateTime { get; set; }

        [JsonProperty("isWorking")]
        public long IsWorking { get; set; }
    }
}
