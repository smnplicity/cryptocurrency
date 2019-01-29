using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Kraken.Model
{
    public class KrakenAsset
    {
        [JsonProperty(PropertyName = "aclass")]
        public string Aclass { get; set; }

        [JsonProperty(PropertyName = "altname")]
        public string AltName { get; set; }

        [JsonProperty(PropertyName = "decimals")]
        public int Decimals { get; set; }

        [JsonProperty(PropertyName = "display_decimals")]
        public int DisplayDecimals { get; set; }
    }

    public class KrakenAssetPair
    {
        [JsonProperty(PropertyName = "altname")]
        public string AltName { get; set; }

        [JsonProperty(PropertyName = "base")]
        public string Base { get; set; }

        [JsonProperty(PropertyName = "quote")]
        public string Quote { get; set; }
    }
}
