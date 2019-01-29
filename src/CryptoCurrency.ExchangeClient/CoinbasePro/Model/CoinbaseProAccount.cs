using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.CoinbasePro.Model
{
    public class CoinbaseProAccount
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "balance")]
        public double Balance { get; set; }

        [JsonProperty(PropertyName = "available")]
        public double Available { get; set; }

        [JsonProperty(PropertyName = "hold")]
        public double Hold { get; set; }

        [JsonProperty(PropertyName = "profile_id")]
        public string ProfileId { get; set; }
    }
}
