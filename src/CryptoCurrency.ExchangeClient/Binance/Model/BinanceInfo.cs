using System.Collections.Generic;

using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Binance.Model
{
    public class BinanceSymbol
    {
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "baseAsset")]
        public string BaseAsset { get; set; }

        [JsonProperty(PropertyName = "baseAssetPrecision")]
        public int BaseAssetPrecision { get; set; }

        [JsonProperty(PropertyName = "quoteAsset")]
        public string QuoteAsset { get; set; }

        [JsonProperty(PropertyName = "quoteAssetPrecision")]
        public int QuoteAssetPrecision { get; set; }
    }

    public class BinanceInfo
    {
        [JsonProperty(PropertyName = "timezone")]
        public string Timezone { get; set; }

        [JsonProperty(PropertyName = "symbols")]
        public ICollection<BinanceSymbol> Symbols { get; set; }
    }
}
