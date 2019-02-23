using Newtonsoft.Json;

using System.Collections.Generic;

namespace CryptoCurrency.ExchangeClient.Binance.Model
{
    public class BinanceNewOrderFill
    {
        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("qty")]
        public double Quantity { get; set; }

        [JsonProperty("commission")]
        public string Commission { get; set; }

        [JsonProperty("commissionAsset")]
        public string CommissionAsset { get; set; }
    }

    public class BinanceNewOrder
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }
        
        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; set; }
        
        [JsonProperty("transactTime")]
        public long TransactTime { get; set; }

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

        [JsonProperty("fills")]
        public ICollection<BinanceNewOrderFill> Fills { get; set; }
    }
}
