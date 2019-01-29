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
    public class ExchangeTradeAggregateWorker : IExchangeTradeAggregateWorker
    {
        private ILoggerFactory LoggerFactory { get; set; }

        private ISymbolFactory SymbolFactory { get; set; }

        private IStorageTransactionFactory<HistorianDbContext> StorageTransactionFactory { get; set; }

        private IHistorianRepository HistorianRepository { get; set; }

        private IMarketRepository MarketRepository { get; set; }

        private IExchangeWorker ExchangeWorker { get; set; }
        
        private IExchange Exchange { get { return ExchangeWorker.Exchange; } }

        public ExchangeTradeAggregateWorker(
            ILoggerFactory loggerFactory,
            ISymbolFactory symbolFactory,
            IStorageTransactionFactory<HistorianDbContext> storageTransactionFactory,
            IHistorianRepository historianRepository, 
            IMarketRepository marketRepository)
        {
            LoggerFactory = loggerFactory;
            SymbolFactory = symbolFactory;
            StorageTransactionFactory = storageTransactionFactory;
            HistorianRepository = historianRepository;
            MarketRepository = marketRepository;
        }

        public void Start(IExchangeWorker exchangeWorker, int pageSize)
        {
            ExchangeWorker = exchangeWorker;

            foreach (var symbolCode in Exchange.Symbol)
            {
                var symbol = SymbolFactory.Get(symbolCode);

                if (symbol.Tradable)
                    AggregateTrades(symbol, pageSize);
            }
        }

        private void AggregateTrades(ISymbol symbol, int processLimit) => Task.Run(async () =>
        {
            var logger = LoggerFactory.CreateLogger($"Historian.{Exchange.Name}.{symbol.Code}.Worker.TradeAggregate");

            using (logger.BeginExchangeScope(Exchange.Name))
            {
                using (logger.BeginSymbolScope(symbol.Code))
                {
                    var tradeId = (await HistorianRepository.GetLastTradeId(Exchange.Name, symbol.Code)).GetValueOrDefault(0);

                    while (true)
                    {
                        try
                        {
                            var s = DateTime.Now;

                            var trades = await MarketRepository.GetNextTrades(Exchange.Name, symbol.Code, tradeId, processLimit);

                            if (trades.Count > 0)
                            {
                                using (var transaction = await StorageTransactionFactory.Begin())
                                {
                                    await MarketRepository.SaveTradeAggregates(transaction, Exchange.Name, symbol.Code, trades);

                                    tradeId = trades.Max(t => t.TradeId);

                                    await HistorianRepository.SetLastTradeId(transaction, Exchange.Name, symbol.Code, tradeId);

                                    await transaction.Commit();
                                }

                                var e = DateTime.Now;

                                logger.LogInformation($"Aggregation up to trade id {tradeId} took {(e.Subtract(s).TotalMilliseconds)}ms.");
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
        });
    }
}