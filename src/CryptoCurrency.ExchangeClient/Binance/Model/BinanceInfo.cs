using System.Collections.Generic;

using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Binance.Model
{
    public class BinanceSymbol
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("baseAsset")]
        public string BaseAsset { get; set; }

        [JsonProperty("baseAssetPrecision")]
        public int BaseAssetPrecision { get; set; }

        [JsonProperty("quoteAsset")]
        public string QuoteAsset { get; set; }

        [JsonProperty("quotePrecision")]
        public int QuoteAssetPrecision { get; set; }

        [JsonProperty("filters")]
        public ICollection<Dictionary<string, object>> Filters { get; set; }
    }

    public class BinanceInfo
    {
        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("symbols")]
        public ICollection<BinanceSymbol> Symbols { get; set; }
    }
}
