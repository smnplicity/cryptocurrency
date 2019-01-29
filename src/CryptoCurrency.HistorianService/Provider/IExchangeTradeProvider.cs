using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.StorageTransaction;

namespace CryptoCurrency.HistorianService.Provider
{
    public interface IExchangeTradeProvider
    {
        Task<TradeResult> ReceiveTradesHttp(IStorageTransaction transaction, ILogger logger, ExchangeEnum exchange, ISymbol symbol, IExchangeHttpClient httpClient, int limit, string lastTradeFilter);

        Task AddTrades(IStorageTransaction transaction, ILogger logger, TradeResult tradeResult);
    }
}
