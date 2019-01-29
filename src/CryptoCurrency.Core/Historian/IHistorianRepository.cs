using System.Collections.Generic;
using System.Threading.Tasks;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Historian.Model;
using CryptoCurrency.Core.StorageTransaction;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Historian
{
    public interface IHistorianRepository
    {
        Task<string> GetTradeFilter(ExchangeEnum exchange, SymbolCodeEnum symbolCode);

        Task SetTradeFilter(IStorageTransaction transaction, ExchangeEnum exchange, SymbolCodeEnum symbolCode, string filter);

        Task<long?> GetLastTradeId(ExchangeEnum exchange, SymbolCodeEnum symbolCode);

        Task SetLastTradeId(IStorageTransaction transaction, ExchangeEnum exchange, SymbolCodeEnum symbolCode, long lastTradeId);

        Task<long?> GetLastTradeStatId(ExchangeEnum exchange, SymbolCodeEnum symbolCode);

        Task SetLastTradeStatId(IStorageTransaction transaction, ExchangeEnum exchange, SymbolCodeEnum symbolCode, long lastTradeStatId);

        Task<ICollection<HistorianExchangeSymbol>> GetSymbols(ExchangeEnum exchange);

        Task AddTradeCatchup(HistorianTradeCatchup catchup);

        Task<HistorianTradeCatchup> GetNextTradeCatchup(ExchangeEnum exchange, SymbolCodeEnum symbolCode);

        Task UpdateTradeCatchup(HistorianTradeCatchup catchup);

        Task RemoveTradeCatchup(HistorianTradeCatchup catchup);

        Task WriteLog(HistorianLogItem logItem);
    }
}