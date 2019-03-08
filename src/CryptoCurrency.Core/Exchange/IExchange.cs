using System.Collections.Generic;
using System.Threading.Tasks;

using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Exchange
{
    public interface IExchange
    {
        ExchangeEnum Name { get; }

        ICollection<ExchangeCurrencyConverter> CurrencyConverter { get; }

        ICollection<SymbolCodeEnum> Symbol { get; }

        ICollection<ExchangeStatsKeyEnum> SupportedStatKeys { get; }

        bool SupportsHistoricalLoad { get; }

        bool Initialized { get; }

        Task Initialize();

        IExchangeHttpClient GetHttpClient();

        IExchangeWebSocketClient GetWebSocketClient();
    }
}
