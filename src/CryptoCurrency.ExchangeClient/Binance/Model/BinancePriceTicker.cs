using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Binance.Model
{
    public class BinancePriceTicker
    {
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }
    }
}
