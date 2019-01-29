using System.Collections.Generic;

using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Kraken.Model
{
    public class KrakenWrappedResponse<T>
    {
        [JsonProperty(PropertyName = "error")]
        public ICollection<string> Error { get; set; }

        [JsonProperty(PropertyName = "result")]
        public T Result { get; set; }
    }
}
