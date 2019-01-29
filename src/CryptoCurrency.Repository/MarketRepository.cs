using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;

using CryptoCurrency.Core;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Interval;
using CryptoCurrency.Core.Market;
using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.StorageTransaction;

using CryptoCurrency.Repository.Edm.Historian;

namespace CryptoCurrency.Repository
{
    public class MarketRepository : IMarketRepository
    {
        private ILogger Logger { get; set; }

        private IIntervalFactory IntervalFactory { get; set; }

        private IDesignTimeDbContextFactory<HistorianDbContext> ContextFactory { get; set; }

        private IStorageTransactionFactory<HistorianDbContext> StorageTransactionFactory { get; set; }

        public MarketRepository(
            ILoggerFactory loggerFactory, 
            IIntervalFactory intervalFactory, 
            IDesignTimeDbContextFactory<HistorianDbContext> contextFactory,
            IStorageTransactionFactory<HistorianDbContext> storageTransactionFactory)
        {
            IntervalFactory = intervalFactory;
            ContextFactory = contextFactory;
            StorageTransactionFactory = storageTransactionFactory;

            Logger = loggerFactory.CreateLogger<MarketRepository>();
        }

        public async Task<ICollection<MarketTrade>> GetTrades(ExchangeEnum exchange, SymbolCodeEnum symbolCode, Epoch from, Epoch to)
        {
            using (var context = ContextFactory.CreateDbContext(null))
            {
                var trades =
                    from
                        t
                    in
                        context.ExchangeTrade
                    where
                        t.ExchangeId == (int)exchange &&
                        t.SymbolId == (int)symbolCode &&
                        t.Timestamp >= @from.TimestampMilliseconds &&
                        t.Timestamp <= to.TimestampMilliseconds
                    select new
                    {
                        Exchange = exchange,
                        SymbolCode = symbolCode,
                        Timestamp = t.Timestamp,
                        TradeId = t.TradeId,
                        Side = t.OrderSideId.HasValue ? (OrderSideEnum)t.OrderSideId : (OrderSideEnum?)null,
                        Price = t.Price,
                        Volume = t.Volume
                    };

                var raw = await trades.ToListAsync();

                return raw.Select(t => new MarketTrade
                {
                    Exchange = t.Exchange,
                    SymbolCode = t.SymbolCode,
                    Epoch = Epoch.FromMilliseconds(t.Timestamp),
                    TradeId = t.TradeId,
                    Side = t.Side,
                    Price = t.Price,
                    Volume = t.Volume
                }).ToList();
            }
        }

        public async Task SaveTrades(IStorageTransaction transaction, ICollection<MarketTrade> trades)
        {
            var context = (HistorianDbContext)transaction.GetContext();

            using (var cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.Transaction = context.Database.CurrentTransaction.GetDbTransaction();
                
                var sql = @"insert ignore into `exchange_trade`
                (`exchange_id`,
                `symbol_id`,
                `timestamp`,
                `order_side_id`,
                `price`,
                `volume`,
                `source_trade_id`) values ";

                sql += string.Join(",", trades.Select(trade => $"({(int)trade.Exchange}," +
                $"{(int)trade.SymbolCode}," +
                $"{trade.Epoch.TimestampMilliseconds}," +
                $"{(trade.Side.HasValue ? ((int)trade.Side).ToString() : "NULL")}," +
                $"{trade.Price}," +
                $"{trade.Volume}," +
                $"{(trade.SourceTradeId != null ? trade.SourceTradeId : "NULL")})"));

                cmd.CommandText = sql;

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<ICollection<MarketTrade>> GetNextTrades(ExchangeEnum exchange, SymbolCodeEnum symbolCode, long tradeId, int limit = 1)
        {
            using (var context = ContextFactory.CreateDbContext(null))
            {
                var trades = context.ExchangeTrade
                    .Where(t => t.ExchangeId == (int)exchange && t.SymbolId == (int)symbolCode && t.TradeId > tradeId)
                    .OrderBy(t => t.TradeId)
                    .Take(limit)
                    .Select(t => new
                    {
                        Exchange = exchange,
                        SymbolCode = symbolCode,
                        Timestamp = t.Timestamp,
                        TradeId = (long)t.TradeId,
                        Side = t.OrderSideId.HasValue ? (OrderSideEnum)t.OrderSideId : (OrderSideEnum?)null,
                        Price = t.Price,
                        Volume = t.Volume
                    });
                
                var raw = await trades.ToListAsync();

                return raw.Select(t => new MarketTrade
                {
                    Exchange = t.Exchange,
                    SymbolCode = t.SymbolCode,
                    Epoch = Epoch.FromMilliseconds(t.Timestamp),
                    TradeId = t.TradeId,
                    Side = t.Side,
                    Price = t.Price,
                    Volume = t.Volume
                }).ToList();
            }
        }

        public async Task<ICollection<MarketAggregate>> GetTradeAggregates(ExchangeEnum exchange, SymbolCodeEnum symbolCode, IntervalKey intervalKey, Epoch from, int dataPoints)
        {
            var intervals = IntervalFactory.GenerateIntervals(intervalKey, from, dataPoints);

            var min = intervals.First();
            var max = intervals.Last();

            using (var context = ContextFactory.CreateDbContext(null))
            {
                var aggs = await context.ExchangeTradeAggregate
                    .Where(a => a.ExchangeId == (int)exchange && a.SymbolId == (int)symbolCode && a.IntervalKey == intervalKey.Key && a.Timestamp >= min.From.TimestampMilliseconds && a.Timestamp <= max.From.TimestampMilliseconds)
                    .ToListAsync();

                return aggs.Select(a => new MarketAggregate
                {
                    Exchange = exchange,
                    Symbol = symbolCode,
                    IntervalKey = a.IntervalKey,
                    Epoch = Epoch.FromMilliseconds(a.Timestamp),
                    Open = a.Open,
                    OpenEpoch = Epoch.FromMilliseconds(a.OpenTimestamp),
                    High = a.High,
                    Low = a.Low,
                    Close = a.Close,
                    CloseEpoch = Epoch.FromMilliseconds(a.CloseTimestamp),
                    BuyVolume = a.BuyVolume,
                    SellVolume = a.SellVolume,
                    TotalVolume = a.TotalVolume,
                    BuyCount = a.BuyCount,
                    SellCount = a.SellCount,
                    TotalCount = a.TotalCount
                }).ToList();
            }
        }

        private class TradeCartesian : MarketTrade
        {
            public IntervalKey IntervalKey { get; set; }

            public Epoch IntervalEpoch { get; set; }
        }

        public async Task SaveTradeAggregates(IStorageTransaction transaction, ExchangeEnum exchange, SymbolCodeEnum symbolCode, ICollection<MarketTrade> trades)
        {
            var min = trades.Min(t => t.Epoch);
            var max = trades.Max(t => t.Epoch);

            var expanded = new List<TradeCartesian>();

            foreach(var group in IntervalFactory.ListGroups())
            {
                foreach(var ik in IntervalFactory.ListIntervalKeys(group.IntervalGroup))
                {
                    foreach(var trade in trades)
                    {
                        var period = IntervalFactory.GenerateIntervals(ik, trade.Epoch, trade.Epoch).FirstOrDefault();

                        expanded.Add(new TradeCartesian
                        {
                            IntervalKey = period.IntervalKey,
                            IntervalEpoch = period.From,
                            Exchange = trade.Exchange,
                            SymbolCode = trade.SymbolCode,
                            Epoch = trade.Epoch,
                            TradeId = trade.TradeId,
                            Price = trade.Price,
                            Volume = trade.Volume,
                            Side = trade.Side
                        });

                    }
                }
            }

            var context = (HistorianDbContext)transaction.GetContext();

            using (var cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.Transaction = context.Database.CurrentTransaction.GetDbTransaction();

                var sql = @"insert into `exchange_trade_aggregate`
                (`exchange_id`,
                `symbol_id`,
                `interval_key`,
                `timestamp`,
                `open`,
                `open_timestamp`,
                `high`,
                `low`,
                `close`,
                `close_timestamp`,
                `buy_volume`,
                `sell_volume`,
                `total_volume`,
                `buy_count`,
                `sell_count`,
                `total_count`) values ";

                sql += string.Join(",\r\n", expanded.Select(t => $"(" +
                    $"{(int)t.Exchange}," +
                    $"{(int)t.SymbolCode}," +
                    $"'{t.IntervalKey.Key}'," +
                    $"{t.IntervalEpoch.TimestampMilliseconds}," +
                    $"{t.Price}," +
                    $"{t.Epoch.TimestampMilliseconds}," +
                    $"{t.Price}," +
                    $"{t.Price}," +
                    $"{t.Price}," +
                    $"{t.Epoch.TimestampMilliseconds}," +
                    $"{(t.Side == OrderSideEnum.Buy ? t.Volume.ToString("R") : "NULL")}," +
                    $"{(t.Side == OrderSideEnum.Sell ? t.Volume.ToString("R") : "NULL")}," +
                    $"{t.Volume}," +
                    $"{(t.Side == OrderSideEnum.Buy ? "1" : "NULL")}," +
                    $"{(t.Side == OrderSideEnum.Sell ? "1" : "NULL")}," +
                    $"1)"));

                sql += @"
                on duplicate key update
	                `open` = case when values(`open_timestamp`) < `open_timestamp` then values(`open`) else `open` end,
	                `open_timestamp` = case when values(`open_timestamp`) < `open_timestamp` then values(`open_timestamp`) else `open_timestamp` end,
                    `high` = case when values(`high`) > `high` then values(`high`) else `high` end,
                    `low` = case when values(`low`) < `low` then values(`low`) else `low` end,
                    `close` = case when values(`close_timestamp`) > `close_timestamp` then values(`close`) else `close` end,
	                `close_timestamp` = case when values(`close_timestamp`) > `close_timestamp` then values(`close_timestamp`) else `close_timestamp` end,
	                `buy_volume` = case when values(`buy_volume`) is not null then ifnull(`buy_volume`, 0) + values(`buy_volume`) else `buy_volume` end,
                    `sell_volume` = case when values(`sell_volume`) is not null then ifnull(`sell_volume`, 0) + values(`sell_volume`) else `sell_volume` end,
                    `total_volume` = `total_volume` + values(`total_volume`),
                    `buy_count` = case when values(`buy_count`) is not null then ifnull(`buy_count`, 0) + values(`buy_count`) else `buy_count` end,
                    `sell_count` = case when values(`sell_count`) is not null then ifnull(`sell_count`, 0) + values(`sell_count`) else `sell_count` end,
                    `total_count` = `total_count` + values(`total_count`)";

                cmd.CommandText = sql;

                await cmd.ExecuteNonQueryAsync();
            }
        }
        
        public async Task SaveTradeStats(IStorageTransaction transaction, ICollection<MarketTradeStat> tradeStats)
        {
            using (var context = ContextFactory.CreateDbContext(null))
            {
                using (var cmd = context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = $@"insert ignore into `exchange_trade_stat`
                    (`exchange_id`,
                    `symbol_id`,
                    `stat_key_id`,
                    `timestamp`,
                    `value`)
                    values {string.Join(",\r\n", tradeStats.Select(t => $"({(int)t.Exchange},{(int)t.SymbolCode},{(int)t.StatKey},{t.Epoch.TimestampMilliseconds},{t.Value})"))}
                    on duplicate key update `value` = values(`value`)";

                    await cmd.Connection.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();

                    cmd.Connection.Close();
                }
            }
        }

        public async Task<ICollection<MarketTradeStat>> GetNextTradeStats(ExchangeEnum exchange, SymbolCodeEnum symbolCode, ExchangeStatsKeyEnum statKey, long tradeStatId, int limit = 1)
        {
            using (var context = ContextFactory.CreateDbContext(null))
            {
                var tradeStats = context.ExchangeTradeStat
                    .Where(t => t.ExchangeId == (int)exchange && t.SymbolId == (int)symbolCode && t.StatKeyId == (int)statKey && t.TradeStatId > tradeStatId)
                    .OrderBy(t => t.TradeStatId)
                    .Take(limit)
                    .Select(t => new
                    {
                        Exchange = exchange,
                        SymbolCode = symbolCode,
                        StatKey = statKey,
                        Timestamp = t.Timestamp,
                        TradeStatId = (long)t.TradeStatId,
                        Value = t.Value
                    });

                var raw = await tradeStats.ToListAsync();

                return raw.Select(t => new MarketTradeStat
                {
                    Exchange = t.Exchange,
                    SymbolCode = t.SymbolCode,
                    StatKey = t.StatKey,
                    Epoch = Epoch.FromMilliseconds(t.Timestamp),
                    TradeStatId = t.TradeStatId,
                    Value = t.Value
                }).ToList();
            }
        }

        private class TradeStatCartesian : MarketTradeStat
        {
            public IntervalKey IntervalKey { get; set; }

            public Epoch IntervalEpoch { get; set; }
        }
        
        public async Task SaveTradeStatAggregates(IStorageTransaction transaction, ExchangeEnum exchange, SymbolCodeEnum symbolCode, ICollection<MarketTradeStat> tradeStats)
        {
            var min = tradeStats.Min(t => t.Epoch);
            var max = tradeStats.Max(t => t.Epoch);

            var expanded = new List<TradeStatCartesian>();

            foreach (var group in IntervalFactory.ListGroups())
            {
                foreach (var ik in IntervalFactory.ListIntervalKeys(group.IntervalGroup))
                {
                    foreach (var tradeStat in tradeStats)
                    {
                        var period = IntervalFactory.GenerateIntervals(ik, tradeStat.Epoch, tradeStat.Epoch).FirstOrDefault();

                        expanded.Add(new TradeStatCartesian
                        {
                            IntervalKey = period.IntervalKey,
                            IntervalEpoch = period.From,
                            Exchange = tradeStat.Exchange,
                            SymbolCode = symbolCode,
                            StatKey = tradeStat.StatKey,
                            Epoch = tradeStat.Epoch,
                            Value = tradeStat.Value
                        });

                    }
                }
            }

            using (var context = ContextFactory.CreateDbContext(null))
            {
                using (var cmd = context.Database.GetDbConnection().CreateCommand())
                {
                    var sql = @"insert into `exchange_trade_aggregate`
                    (`exchange_id`,
                    `symbol_id`,
                    `interval_key`,
                    `timestamp`,
                    `open`,
                    `open_timestamp`,
                    `high`,
                    `low`,
                    `close`,
                    `close_timestamp`,
                    `total_count`) values ";

                    sql += string.Join(",\r\n", expanded.Select(t => $"(" +
                        $"{(int)t.Exchange}," +
                        $"{(int)t.SymbolCode}," +
                        $"'{t.IntervalKey.Key}'," +
                        $"{t.IntervalEpoch.TimestampMilliseconds}," +
                        $"{t.Value}," +
                        $"{t.Epoch.TimestampMilliseconds}," +
                        $"{t.Value}," +
                        $"{t.Value}," +
                        $"{t.Value}," +
                        $"{t.Epoch.TimestampMilliseconds}," +
                        $"1)"));

                    sql += @"
                    on duplicate key update
	                    `open` = case when values(`open_timestamp`) < `open_timestamp` then values(`open`) else `open` end,
	                    `open_timestamp` = case when values(`open_timestamp`) < `open_timestamp` then values(`open_timestamp`) else `open_timestamp` end,
                        `high` = case when values(`high`) > `high` then values(`high`) else `high` end,
                        `low` = case when values(`low`) < `low` then values(`low`) else `low` end,
                        `close` = case when values(`close_timestamp`) > `close_timestamp` then values(`close`) else `close` end,
	                    `close_timestamp` = case when values(`close_timestamp`) > `close_timestamp` then values(`close_timestamp`) else `close_timestamp` end,
	                    `total_count` = `total_count` + values(`total_count`)";

                     cmd.CommandText = sql;

                    await cmd.Connection.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();

                    cmd.Connection.Close();
                }
            }
        }
    }
}