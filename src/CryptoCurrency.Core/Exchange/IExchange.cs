using System.Collections.Generic;

using Newtonsoft.Json;

using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Exchange
{
    public interface IExchange
    {
        [JsonProperty("name")]
        ExchangeEnum Name { get; }

        [JsonProperty("currency")]
        ICollection<ExchangeCurrency> Currency { get; }

        [JsonProperty("symbol")]
        ICollection<SymbolCodeEnum> Symbol { get; }

        [JsonIgnore]
        ICollection<ExchangeStatsKeyEnum> SupportedStatKeys { get; }

        [JsonIgnore]
        bool SupportsHistoricalLoad { get; }

        IExchangeHttpClient GetHttpClient();

        IExchangeWebSocketClient GetWebSocketClient();
    }
}
