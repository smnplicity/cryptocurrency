using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Binance.Http
{
    public class BinanceApiError
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        [JsonProperty(PropertyName = "msg")]
        public string Message { get; set; }
    }
}