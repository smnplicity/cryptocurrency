using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.CoinbasePro.Model
{
    public class CoinbaseProRequestError
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
