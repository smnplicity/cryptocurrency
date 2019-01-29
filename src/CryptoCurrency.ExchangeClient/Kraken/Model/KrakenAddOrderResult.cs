using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Kraken.Model
{
    public class KrakenAddOrderResult
    {
        [JsonProperty(PropertyName = "desc")]
        public AddOrderDescription Desc { get; set; }

        [JsonProperty(PropertyName = "txid")]
        public string[] TxId { get; set; }
    }

    public class AddOrderDescription
    {
        [JsonProperty(PropertyName = "order")]
        public string Order { get; set; }

        [JsonProperty(PropertyName = "close")]
        public string Close { get; set; }
    }
}
