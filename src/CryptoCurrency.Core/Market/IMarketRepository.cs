using System.Collections.Generic;
using System.Threading.Tasks;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Interval;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.StorageTransaction;

namespace CryptoCurrency.Core.Market
{
    public interface IMarketRepository
    {
        Task<MarketTick> GetTick(ExchangeEnum exchange, SymbolCodeEnum symbolCode, Epoch at);

        Task<MarketTickAverage> GetTickAverage(ICollection<ExchangeEnum> exchanges, SymbolCodeEnum symbolCode, Epoch at);

        Task SaveTrades(IStorageTransaction transaction, ICollection<MarketTrade> trades);

        Task<PagedCollection<MarketTrade>> GetTrades(ExchangeEnum exchange, SymbolCodeEnum symbolCode, Epoch from, Epoch to, int? pageSize, int? pageNumber);

        Task<ICollection<MarketTrade>> GetNextTrades(ExchangeEnum exchange, SymbolCodeEnum symbolCode, long tradeId, int limit);

        Task<ICollection<MarketTrade>> GetNextTrades(ExchangeEnum exchange, SymbolCodeEnum symbolCode, Epoch from, long tradeId, int limit);

        Task SaveTradeAggregates(IStorageTransaction transaction, ExchangeEnum exchange, SymbolCodeEnum symbolCode, ICollection<MarketTrade> trades);

        Task<ICollection<MarketAggregate>> GetTradeAggregates(ExchangeEnum exchange, SymbolCodeEnum symbolCode, IntervalKey intervalKey, Epoch from, int dataPoints);

        Task<PagedCollection<MarketAggregate>> GetTradeAggregates(ExchangeEnum exchange, SymbolCodeEnum symbolCode, IntervalKey intervalKey, Epoch from, Epoch to, int? pageSize, int? pageNumber);

        Task<ICollection<MarketTradeStat>> GetNextTradeStats(ExchangeEnum exchange, SymbolCodeEnum symbolCode, ExchangeStatsKeyEnum statKey, long tradeStatId, int limit = 1);

        Task SaveTradeStats(IStorageTransaction transaction, ICollection<MarketTradeStat> tradeStats);

        Task SaveTradeStatAggregates(IStorageTransaction transaction, ExchangeEnum exchange, SymbolCodeEnum symbolCode, ICollection<MarketTradeStat> tradeStats);
    }
}