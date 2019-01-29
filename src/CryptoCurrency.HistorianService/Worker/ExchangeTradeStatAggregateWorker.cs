using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Historian;
using CryptoCurrency.Core.Market;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.StorageTransaction;

using CryptoCurrency.HistorianService.Extension;

using CryptoCurrency.Repository.Edm.Historian;

namespace CryptoCurrency.HistorianService.Worker
{
    public class ExchangeTradeStatAggregateWorker : IExchangeTradeStatAggregateWorker
    {
        private ILoggerFactory LoggerFactory { get; set; }

        private ISymbolFactory SymbolFactory { get; set; }

        private IStorageTransactionFactory<HistorianDbContext> StorageTransactionFactory { get; set; }

        private IHistorianRepository HistorianRepository { get; set; }

        private IMarketRepository MarketRepository { get; set; }

        private IExchangeTradeStatProvider ExchangeTradeStatProvider { get; set; }

        private IExchangeWorker ExchangeWorker { get; set; }

        private IExchange Exchange { get { return ExchangeWorker.Exchange; } }

        public ExchangeTradeStatAggregateWorker(
            ILoggerFactory loggerFactory,
            ISymbolFactory symbolFactory,
            IStorageTransactionFactory<HistorianDbContext> storageTransactionFactory,
            IExchangeTradeStatProvider exchangeTradeStatProvider,
            IHistorianRepository historianRepository, 
            IMarketRepository marketRepository)
        {
            LoggerFactory = loggerFactory;
            SymbolFactory = symbolFactory;
            StorageTransactionFactory = storageTransactionFactory;
            ExchangeTradeStatProvider = exchangeTradeStatProvider;
            HistorianRepository = historianRepository;
            MarketRepository = marketRepository;
        }

        public void Start(IExchangeWorker exchangeWorker, int pageSize)
        {
            ExchangeWorker = exchangeWorker;

            if (Exchange.SupportedStatKeys == null)
                return;

            foreach (var symbolCode in Exchange.Symbol)
            {
                var symbol = SymbolFactory.Get(symbolCode);

                if (symbol.Tradable)
                {
                    foreach (var statsKey in Exchange.SupportedStatKeys)
                    {
                        AggregateTradeStats(symbol, statsKey, pageSize);
                    }
                }
            }
        }

        private void AggregateTradeStats(ISymbol baseSymbol, ExchangeStatsKeyEnum statsKey, int processLimit) => Task.Run(async () =>
        {
            var fullSymbolCode = ExchangeTradeStatProvider.Convert(baseSymbol.Code, statsKey);

            var logger = LoggerFactory.CreateLogger($"Historian.{Exchange.Name}.{fullSymbolCode}.Worker.TradeStatAggregate");

            using (logger.BeginExchangeScope(Exchange.Name))
            {
                using (logger.BeginSymbolScope(fullSymbolCode))
                {
                    using (logger.BeginExchangeStatsScope(statsKey))
                    {
                        var tradeStatId = (await HistorianRepository.GetLastTradeStatId(Exchange.Name, fullSymbolCode)).GetValueOrDefault(0);

                        while (true)
                        {
                            try
                            {
                                var s = DateTime.Now;

                                var tradeStats = await MarketRepository.GetNextTradeStats(Exchange.Name, baseSymbol.Code, statsKey, tradeStatId, processLimit);

                                if (tradeStats.Count > 0)
                                {
                                    using (var transaction = await StorageTransactionFactory.Begin())
                                    {
                                        await MarketRepository.SaveTradeStatAggregates(transaction, Exchange.Name, fullSymbolCode, tradeStats);

                                        tradeStatId = tradeStats.Max(t => t.TradeStatId);

                                        await HistorianRepository.SetLastTradeStatId(transaction, Exchange.Name, fullSymbolCode, tradeStatId);

                                        await transaction.Commit();
                                    }

                                    var e = DateTime.Now;

                                    logger.LogInformation($"Aggregation up to trade stat id {tradeStatId} took {(e.Subtract(s).TotalMilliseconds)}ms.");
                                }

                                await Task.Delay(5);
                            }
                            catch (Exception ex)
                            {
                                logger.LogCritical(ex, "Aggregation failed.");

                                await Task.Delay(250);
                            }
                        }
                    }
                }
            }
        });
    }
}
