using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Historian;
using CryptoCurrency.Core.Market;
using CryptoCurrency.Core.Symbol;

using CryptoCurrency.HistorianService.Provider;
using CryptoCurrency.HistorianService.Extension;
using CryptoCurrency.Repository.Edm.Historian;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.StorageTransaction;

namespace CryptoCurrency.HistorianService.Worker
{
    public class ExchangeTradeWorker : IExchangeTradeWorker
    {
        private ILoggerFactory LoggerFactory { get; set; }

        private ISymbolFactory SymbolFactory { get; set; }
        
        private IStorageTransactionFactory<HistorianDbContext> StorageTransactionFactory { get; set; }

        private IMarketRepository MarketRepository { get; set; }

        private IHistorianRepository HistorianRepository { get; set; }

        private IExchangeTradeProvider ExchangeTradeProvider { get; set; }

        private IExchangeTradeCatchupWorker ExchangeTradeCatchupWorker { get; set; }

        private IExchangeWorker ExchangeWorker { get; set; }

        private ILogger Logger { get; set; }

        private IExchange Exchange { get { return ExchangeWorker.Exchange; } }

        private IExchangeHttpClient HttpClient { get; set; }

        private IExchangeWebSocketClient WebSocketClient { get; set; }

        private int Limit { get; set; }

        public ExchangeTradeWorker
            (ILoggerFactory loggerFactory,
            ISymbolFactory symbolFactory,
            IStorageTransactionFactory<HistorianDbContext> storageTransactionFactory,
            IMarketRepository marketRepository,
            IHistorianRepository historianRepository,
            IExchangeTradeProvider exchangeTradeProvider,
            IExchangeTradeCatchupWorker exchangeTradeCatchupWorker)
        {
            LoggerFactory = loggerFactory;
            SymbolFactory = symbolFactory;
            StorageTransactionFactory = storageTransactionFactory;
            MarketRepository = marketRepository;
            HistorianRepository = historianRepository;
            ExchangeTradeProvider = exchangeTradeProvider;
            ExchangeTradeCatchupWorker = exchangeTradeCatchupWorker;
        }

        public void Start(IExchangeWorker exchangeWorker, int limit)
        {
            ExchangeWorker = exchangeWorker;
            Limit = limit;

            HttpClient = Exchange.GetHttpClient();
            WebSocketClient = Exchange.GetWebSocketClient();

            Logger = LoggerFactory.CreateLogger($"Historian.{Exchange.Name}.Worker.Trades");

            using (Logger.BeginExchangeScope(Exchange.Name))
            {
                if (WebSocketClient != null && exchangeWorker.Configuration.UseWebSocket)
                    BeginReceiveTradesWebSocket();
                else
                    BeginReceiveTradesHttp();
            }
        }

        #region Http Client
        private void BeginReceiveTradesHttp() => Task.Run(async () =>
        {
            using (Logger.BeginProtocolScope("Https"))
            {
                var symbols = ExchangeWorker.Configuration.Symbol.Select(symbolCode => SymbolFactory.Get(symbolCode)).Where(symbol => symbol.Tradable);

                while (true)
                {
                    if (!ExchangeWorker.Online)
                    {
                        await Task.Delay(1000);

                        continue;
                    }

                    foreach (var symbol in symbols)
                    {
                        using (Logger.BeginSymbolScope(symbol.Code))
                        {
                            try
                            {
                                var lastTradeFilter = await HistorianRepository.GetTradeFilter(Exchange.Name, symbol.Code);

                                TradeResult result;

                                using (var transaction = await StorageTransactionFactory.Begin())
                                {
                                    result = await ExchangeTradeProvider.ReceiveTradesHttp(transaction, Logger, Exchange.Name, symbol, HttpClient, Limit, lastTradeFilter);

                                    if (result != null)
                                        await HistorianRepository.SetTradeFilter(transaction, Exchange.Name, symbol.Code, result.Filter);

                                    await transaction.Commit();
                                }

                                if (result == null)
                                    await Task.Delay(1000);
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError(ex, "Unexpected error occurred");
                            }
                        }
                    }
                }
            }
        });        
        #endregion

        #region Web Socket Client
        private void BeginReceiveTradesWebSocket() => Task.Run(() =>
        {
            ExchangeTradeCatchupWorker.Start(ExchangeWorker, Limit);

            var symbols = ExchangeWorker.Configuration.Symbol.Select(s => SymbolFactory.Get(s)).ToList();

            using (Logger.BeginProtocolScope("Web Socket"))
            {
                Logger.LogInformation($"Establishing connection");

                WebSocketClient.OnOpen += delegate (object sender, EventArgs e)
                {
                    Logger.LogInformation($"Established connection");

                    Logger.LogInformation($"Begin listening for trades");

                    if(WebSocketClient.IsSubscribeModel)
                        WebSocketClient.BeginListenTrades(symbols);
                };

                WebSocketClient.OnClose += delegate (object sender, CloseEventArgs e)
                {
                    if (ExchangeWorker.Online)
                    {
                        Logger.LogWarning($"Connection has been closed");
                    }

                    RetryWebSocketConnect();
                };

                WebSocketClient.OnTradesReceived += async delegate (object sender, TradesReceivedEventArgs e)
                {
                    using (Logger.BeginSymbolScope(e.Data.SymbolCode))
                    {
                        Logger.LogInformation($"Trades received");

                        using (var transaction = await StorageTransactionFactory.Begin())
                        {
                            await ExchangeTradeProvider.AddTrades(transaction, Logger, e.Data);

                            await HistorianRepository.SetTradeFilter(transaction, Exchange.Name, e.Data.SymbolCode, e.Data.Filter);

                            await transaction.Commit();
                        }
                    }
                };

                WebSocketClient.Begin();

                if (!WebSocketClient.IsSubscribeModel)
                    WebSocketClient.BeginListenTrades(symbols);
            }
        });

        private void RetryWebSocketConnect() => Task.Run(async () =>
        {
            await Task.Delay(1000);

            WebSocketClient.Connect();
        });
        #endregion
    }
}
