using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using CryptoCurrency.Core.Market;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.StorageTransaction;

using CryptoCurrency.HistorianService.Extension;

using CryptoCurrency.Repository.Edm.Historian;

namespace CryptoCurrency.HistorianService.Worker
{
    public class ExchangeTradeStatWorker : IExchangeTradeStatWorker
    {
        private ILoggerFactory LoggerFactory { get; set; }

        private ISymbolFactory SymbolFactory { get; set; }

        private IStorageTransactionFactory<HistorianDbContext> StorageTransactionFactory { get; set; }

        private IMarketRepository MarketRepository { get; set; }

        private IExchangeWorker ExchangeWorker { get; set; }

        private ILogger Logger { get; set; }

        private IExchange Exchange { get { return ExchangeWorker.Exchange; } }

        private IExchangeHttpClient HttpClient { get; set; }

        public ExchangeTradeStatWorker(
            ILoggerFactory loggerFactory,
            ISymbolFactory symbolFactory,
            IStorageTransactionFactory<HistorianDbContext> storageTransactionFactory,
            IMarketRepository marketRepository)
        {
            LoggerFactory = loggerFactory;
            SymbolFactory = symbolFactory;
            StorageTransactionFactory = storageTransactionFactory;
            MarketRepository = marketRepository;
        }

        public void Start(IExchangeWorker exchangeWorker) => Task.Run(async () =>
        {
            ExchangeWorker = exchangeWorker;

            if (Exchange.SupportedStatKeys == null)
                return;

            Logger = LoggerFactory.CreateLogger($"Historian.{Exchange.Name}.Worker.TradeStats");

            HttpClient = Exchange.GetHttpClient();

            using (Logger.BeginExchangeScope(Exchange.Name))
            {
                using (Logger.BeginProtocolScope("Https"))
                {
                    while (true)
                    {
                        if (!ExchangeWorker.Online)
                        {
                            await Task.Delay(1000);

                            continue;
                        }

                        var symbols = ExchangeWorker.Configuration.Symbol.Select(symbolCode => SymbolFactory.Get(symbolCode)).Where(s => s.Tradable);

                        foreach (var symbol in symbols)
                        {
                            using (Logger.BeginSymbolScope(symbol.Code))
                            {
                                foreach (var statsKey in Exchange.SupportedStatKeys)
                                {
                                    using (Logger.BeginExchangeStatsScope(statsKey))
                                    {
                                        await ReceiveTradeStatsHttp(symbol, statsKey);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        });

        private async Task ReceiveTradeStatsHttp(ISymbol symbol, ExchangeStatsKeyEnum statsKey)
        {
            Logger.LogInformation("Requesting stats");

            var response = await HttpClient.GetStats(symbol, statsKey);

            var tradeStats = response.Data;

            if (response.StatusCode != WrappedResponseStatusCode.Ok)
            {
                var errorCode = !string.IsNullOrEmpty(response.ErrorCode) ? $"Error Code: {response.ErrorCode} Message: " : "";

                Logger.LogWarning($"Unable to get stats: {errorCode}{response.ErrorMessage}");
                
                return;
            }

            if (tradeStats.Count > 0)
            {
                try
                {
                    var marketTradeStats = tradeStats.Select(s => new MarketTradeStat
                    {
                        Exchange = Exchange.Name,
                        SymbolCode = symbol.Code,
                        Epoch = s.Epoch,
                        StatKey = s.StatKey,
                        Value = s.Value
                    }).OrderBy(t => t.Epoch.TimestampMilliseconds).ToList();

                    using (var transaction = await StorageTransactionFactory.Begin())
                    {
                        await MarketRepository.SaveTradeStats(transaction, marketTradeStats);

                        await transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Unable to save trade stats.");
                }
            }
        }
    }
}
