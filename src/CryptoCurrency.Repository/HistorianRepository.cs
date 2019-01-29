using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

using CryptoCurrency.Core;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Historian;
using CryptoCurrency.Core.Historian.Model;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.StorageTransaction;

using CryptoCurrency.Repository.Edm.Historian;

namespace CryptoCurrency.Repository
{
    public class HistorianRepository : IHistorianRepository
    {
        private ILoggerFactory LoggerFactory { get; set; }

        private IDesignTimeDbContextFactory<HistorianDbContext> ContextFactory { get; set; }

        private IStorageTransactionFactory<HistorianDbContext> StorageTransactionFactory { get; set; }

        public HistorianRepository(
            ILoggerFactory loggerFactory, 
            IDesignTimeDbContextFactory<HistorianDbContext> contextFactory,
            IStorageTransactionFactory<HistorianDbContext> storageTransactionFactory)
        {
            LoggerFactory = loggerFactory;
            ContextFactory = contextFactory;
            StorageTransactionFactory = storageTransactionFactory;
        }

        private async Task<HistorianExchangeSymbolEntity> GetSymbol(ExchangeEnum exchange, SymbolCodeEnum symbolCode)
        {
            using (var context = ContextFactory.CreateDbContext(null))
            {
                return await context.HistorianExchangeSymbol.FindAsync((int)exchange, (int)symbolCode);
            }
        }

        public async Task<long?> GetLastTradeId(ExchangeEnum exchange, SymbolCodeEnum symbolCode)
        {
            var history = await GetSymbol(exchange, symbolCode);

            return history != null ? history.LastTradeId : null;
        }

        public async Task SetLastTradeId(IStorageTransaction transaction, ExchangeEnum exchange, SymbolCodeEnum symbolCode, long lastTradeId)
        {
            var context = (HistorianDbContext)transaction.GetContext();

            var history = await context.HistorianExchangeSymbol.FindAsync((int)exchange, (int)symbolCode);

            if(history == null)
            {
                history = new HistorianExchangeSymbolEntity
                {
                    ExchangeId = (int)exchange,
                    SymbolId = (int)symbolCode
                };

                await context.HistorianExchangeSymbol.AddAsync(history);
            }

            history.LastTradeId = lastTradeId;

            context.Update(history);
        }

        public async Task<long?> GetLastTradeStatId(ExchangeEnum exchange, SymbolCodeEnum symbolCode)
        {
            var history = await GetSymbol(exchange, symbolCode);

            return history != null ? history.LastTradeStatId : null;
        }
        
        public async Task SetLastTradeStatId(IStorageTransaction transaction, ExchangeEnum exchange, SymbolCodeEnum symbolCode, long lastTradeStatId)
        {
            var context = (HistorianDbContext)transaction.GetContext();

            var history = await context.HistorianExchangeSymbol.FindAsync((int)exchange, (int)symbolCode);

            if (history == null)
            {
                history = new HistorianExchangeSymbolEntity
                {
                    ExchangeId = (int)exchange,
                    SymbolId = (int)symbolCode
                };

                await context.HistorianExchangeSymbol.AddAsync(history);
            }

            history.LastTradeStatId = lastTradeStatId;
        }

        public async Task<string> GetTradeFilter(ExchangeEnum exchange, SymbolCodeEnum symbolCode)
        {
            var history = await GetSymbol(exchange, symbolCode);

            return history != null ? history.TradeFilter : null;
        }

        public async Task SetTradeFilter(IStorageTransaction transaction, ExchangeEnum exchange, SymbolCodeEnum symbolCode, string filter)
        {
            var context = (HistorianDbContext)transaction.GetContext();

            var entity = await context.HistorianExchangeSymbol.FindAsync((int)exchange, (int)symbolCode);

            if (entity == null)
            {
                entity = new HistorianExchangeSymbolEntity
                {
                    ExchangeId = (int)exchange,
                    SymbolId = (int)symbolCode,
                    TradeFilter = filter
                };

                await context.HistorianExchangeSymbol.AddAsync(entity);
            }
            else
            {
                entity.TradeFilter = filter;

                context.Update(entity);
            }
        }

        public async Task<ICollection<HistorianExchangeSymbol>> GetSymbols(ExchangeEnum exchange)
        {
            using (var context = ContextFactory.CreateDbContext(null))
            {
                return await context.HistorianExchangeSymbol
                    .Where(s => s.ExchangeId == (int)exchange)
                    .Select(s => new HistorianExchangeSymbol
                    {
                        Exchange = (ExchangeEnum)s.ExchangeId,
                        SymbolCode = (SymbolCodeEnum)s.SymbolId,
                        TradeFilter = s.TradeFilter,
                        LastTradeId = s.LastTradeId,
                        LastTradeStatId = s.LastTradeStatId
                    })
                    .ToListAsync();
            }
        }

        public async Task AddTradeCatchup(HistorianTradeCatchup catchup)
        {
            using (var context = ContextFactory.CreateDbContext(null))
            {
                var entity = await context.HistorianTradeCatchup.FindAsync((int)catchup.Exchange, (int)catchup.SymbolCode, catchup.TradeFilter);

                if (entity == null)
                {
                    await context.HistorianTradeCatchup.AddAsync(new HistorianTradeCatchupEntity
                    {
                        ExchangeId = (int)catchup.Exchange,
                        SymbolId = (int)catchup.SymbolCode,
                        TradeFilter = catchup.TradeFilter,
                        TimestampTo = catchup.EpochTo.TimestampMilliseconds,
                        CurrentTradeFilter = catchup.CurrentTradeFilter,
                        Priority = catchup.Priority
                    });

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<HistorianTradeCatchup> GetNextTradeCatchup(ExchangeEnum exchange, SymbolCodeEnum symbolCode)
        {
            using (var context = ContextFactory.CreateDbContext(null))
            {
                var next = await context.HistorianTradeCatchup
                    .Where(c => c.ExchangeId == (int)exchange && c.SymbolId == (int)symbolCode)
                    .OrderByDescending(c => c.TimestampTo)
                    .OrderBy(c => c.Priority)
                    .FirstOrDefaultAsync();

                return next != null ?
                    new HistorianTradeCatchup
                    {
                        Exchange = (ExchangeEnum)next.ExchangeId,
                        SymbolCode = (SymbolCodeEnum)next.SymbolId,
                        TradeFilter = next.TradeFilter,
                        EpochTo = Epoch.FromMilliseconds(next.TimestampTo),
                        CurrentTradeFilter = next.CurrentTradeFilter,
                        Priority = next.Priority
                    } : null;
            }
        }

        public async Task UpdateTradeCatchup(HistorianTradeCatchup catchup)
        {
            using (var context = ContextFactory.CreateDbContext(null))
            {
                var entity = await context.HistorianTradeCatchup.FindAsync((int)catchup.Exchange, (int)catchup.SymbolCode, catchup.TradeFilter);

                if (entity != null)
                {
                    entity.CurrentTradeFilter = catchup.CurrentTradeFilter;

                    context.HistorianTradeCatchup.Update(entity);

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveTradeCatchup(HistorianTradeCatchup catchup)
        {
            using (var context = ContextFactory.CreateDbContext(null))
            {
                var entity = await context.HistorianTradeCatchup.FindAsync((int)catchup.Exchange, (int)catchup.SymbolCode, catchup.TradeFilter);

                if (entity != null)
                {
                    context.HistorianTradeCatchup.Remove(entity);

                    await context.SaveChangesAsync();
                }
            }
        }
        
        public async Task WriteLog(HistorianLogItem logItem)
        {
            using (var context = ContextFactory.CreateDbContext(null))
            {
                await context.HistorianLog.AddAsync(new HistorianLogEntity
                {
                    Timestamp = logItem.Epoch.TimestampMilliseconds,
                    LevelId = (int)logItem.LogLevel,
                    Category = logItem.Category,
                    Message = logItem.Message,
                    Exception = logItem.Exception != null ? logItem.Exception.ToString() : null,
                    ExchangeId = logItem.Exchange != null ? (int)logItem.Exchange : (int?)null,
                    SymbolId = logItem.SymbolCode != null ? (int)logItem.SymbolCode : (int?)null,
                    Protocol = logItem.Protocol
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
