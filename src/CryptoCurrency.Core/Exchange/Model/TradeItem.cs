using System;

using Newtonsoft.Json;

using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Exchange.Model
{
    public class TradeItem
    {
        [JsonProperty("exchange")]
        public ExchangeEnum Exchange { get; set; }

        [JsonProperty("symbolCode")]
        public SymbolCodeEnum SymbolCode { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("side")]
        public OrderSideEnum Side { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("volume")]
        public double Volume { get; set; }

        [JsonProperty("fee")]
        public double Fee { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }
    }
}
