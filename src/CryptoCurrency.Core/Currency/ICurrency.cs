using Newtonsoft.Json;

namespace CryptoCurrency.Core.Currency
{
    public interface ICurrency
    {
        [JsonProperty("code")]
        CurrencyCodeEnum Code { get; }

        [JsonProperty("symbol")]
        string Symbol { get; }

        [JsonProperty("label")]
        string Label { get; } 
    }
}
