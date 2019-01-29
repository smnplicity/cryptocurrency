using Newtonsoft.Json;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Market
{
    public class Ohlc
    {
        [JsonIgnore]
        public ExchangeEnum Exchange { get; set; }

        [JsonIgnore]
        public SymbolCodeEnum Symbol { get; set; }

        [JsonIgnore]
        public string IntervalKey { get; set; }

        [JsonProperty("epoch")]
        public Epoch Epoch { get; set; }

        [JsonProperty("open")]
        public double Open { get; set; }

        [JsonIgnore]
        public Epoch OpenEpoch { get; set; }

        [JsonProperty("high")]
        public double High { get; set; }

        [JsonProperty("low")]
        public double Low { get; set; }

        [JsonProperty("close")]
        public double Close { get; set; }

        [JsonIgnore]
        public Epoch CloseEpoch { get; set; }
    }
}
