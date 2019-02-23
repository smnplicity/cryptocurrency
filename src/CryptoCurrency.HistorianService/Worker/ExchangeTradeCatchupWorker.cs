using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using CryptoCurrency.Core;
using CryptoCurrency.Core.Historian;
using CryptoCurrency.Core.Historian.Model;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.StorageTransaction;

using CryptoCurrency.HistorianService.Extension;
using CryptoCurrency.HistorianService.Provider;

using CryptoCurrency.Repository.Edm.Historian;
using CryptoCurrency.Core.Exchange.Model;

namespace CryptoCurrency.HistorianService.Worker
{
    public class ExchangeTradeCatchupWorker : IExchangeTradeCatchupWorker
    {
        private ILoggerFactory LoggerFactory { get; set; }
        
        private ISymbolFactory SymbolFactory { get; set; }

        private IStorageTransactionFactory<HistorianDbContext> StorageTransactionFactory { get; set; }

        private IExchangeTradeProvider ExchangeTradeProvider { get; set; }

        private IHistorianRepository HistorianRepository { get; set; }

        private IExchangeWorker ExchangeWorker { get; set; }

        private ILogger Logger { get; set; }

        private IExchange Exchange { get { return ExchangeWorker.Exchange; } }

        private IExchangeHttpClient HttpClient { get; set; }

        public ExchangeTradeCatchupWorker(
            ILoggerFactory loggerFactory,
            ISymbolFactory symbolFactory,
            IStorageTransactionFactory<HistorianDbContext> storageTransactionFactory,
            IExchangeTradeProvider exchangeTradeProvider,
            IHistorianRepository historianRepository)
        {
            LoggerFactory = loggerFactory;
            SymbolFactory = symbolFactory;
            StorageTransactionFactory = storageTransactionFactory;
            ExchangeTradeProvider = exchangeTradeProvider;
            HistorianRepository = historianRepository;
        }

        public void Start(IExchangeWorker exchangeWorker, int limit)
        {
            ExchangeWorker = exchangeWorker;

            HttpClient = Exchange.GetHttpClient();

            Logger = LoggerFactory.CreateLogger($"Historian.{Exchange.Name}.Worker.TradeCatchup");

            using (Logger.BeginExchangeScope(Exchange.Name))
            {
                EnsureHistoricalTrades();

                BeginTradeCatchupWorker(limit);
            }
        }

        private async void EnsureHistoricalTrades()
        {
            if (!Exchange.SupportsHistoricalLoad)
                return;

            Logger.LogInformation("Ensure historical trades are captured.");

            foreach (var symbolCode in ExchangeWorker.Configuration.Symbol)
            {
                var symbol = SymbolFactory.Get(symbolCode);
                var lastTradeFilter = await HistorianRepository.GetTradeFilter(Exchange.Name, symbolCode);

                var priority = string.Equals(lastTradeFilter, Exchange.GetHttpClient().InitialTradeFilter) ? 2 : 1;
                
                var catchup = new HistorianTradeCatchup
                {
                    Exchange = Exchange.Name,
                    SymbolCode = symbolCode,
                    TradeFilter = lastTradeFilter,
                    EpochTo = Epoch.Now,
                    CurrentTradeFilter = lastTradeFilter,
                    Priority = priority
                };

                await HistorianRepository.AddTradeCatchup(catchup);
            }
        }

        private void BeginTradeCatchupWorker(int limit) => Task.Run(async () =>
        {
            using (Logger.BeginProtocolScope("Https"))
            {
                while (true)
                {
                    foreach (var symbolCode in ExchangeWorker.Configuration.Symbol)
                    {
                        using (Logger.BeginSymbolScope(symbolCode))
                        {
                            var symbol = SymbolFactory.Get(symbolCode);

                            if (!symbol.Tradable)
                                continue;

                            try
                            {
                                if (!ExchangeWorker.Online)
                                {
                                    await Task.Delay(2000);

                                    continue;
                                }

                                var current = await HistorianRepository.GetNextTradeCatchup(Exchange.Name, symbolCode);

                                if (current == null)
                                    continue;

                                TradeResult result = null;

                                using (var transaction = await StorageTransactionFactory.Begin())
                                {
                                    result = await ExchangeTradeProvider.ReceiveTradesHttp(transaction, Logger, Exchange.Name, symbol, HttpClient, limit, current.CurrentTradeFilter);

                                    await transaction.Commit();
                                }

                                if (result == null)
                                    continue;

                                var lastTrade = result.Trades.LastOrDefault();

                                if (lastTrade.Epoch.TimestampMilliseconds > current.EpochTo.TimestampMilliseconds)
                                {
                                    await HistorianRepository.RemoveTradeCatchup(current);
                                }
                                else
                                {
                                    current.CurrentTradeFilter = result.Filter;
                                    await HistorianRepository.UpdateTradeCatchup(current);
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError(ex, "Unable to get trade catchup jobs.");
                            }
                        }
                    }
                }
            }
        });
    }
}
