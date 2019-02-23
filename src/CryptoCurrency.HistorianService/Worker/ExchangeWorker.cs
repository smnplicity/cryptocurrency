using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core;
using CryptoCurrency.Core.Historian;
using CryptoCurrency.Core.Historian.Model;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.HistorianService.Extension;

namespace CryptoCurrency.HistorianService.Worker
{
    public class ExchangeWorker : IExchangeWorker
    {
        private ILoggerFactory LoggerFactory { get; set; }

        private ISymbolFactory SymbolFactory { get; set; }

        private IHistorianRepository HistorianRepository { get; set; }

        private IExchangeTradeWorker TradeWorker { get; set; }

        private IExchangeTradeStatWorker TradeStatWorker { get; set; }

        private IExchangeTradeAggregateWorker TradeAggregateWorker { get; set; }

        private IExchangeTradeStatAggregateWorker TradeStatAggregateWorker { get; set; }

        public ILogger Logger { get; set; }

        public IExchange Exchange { get; set; }

        public ExchangeWorkerConfiguration Configuration { get; set; }

        public bool Online { get; set; }

        public ExchangeWorker(
            ILoggerFactory loggerFactory,
            ISymbolFactory symbolFactory,
            IHistorianRepository historianRepository,
            IExchangeTradeWorker tradeWorker,
            IExchangeTradeAggregateWorker tradeAggregateWorker,
            IExchangeTradeStatWorker tradeStatWorker,
            IExchangeTradeStatAggregateWorker tradeStatAggregateWorker)
        {
            LoggerFactory = loggerFactory;
            SymbolFactory = symbolFactory;
            HistorianRepository = historianRepository;

            TradeWorker = tradeWorker;
            TradeStatWorker = tradeStatWorker;
            TradeAggregateWorker = tradeAggregateWorker;
            TradeStatAggregateWorker = tradeStatAggregateWorker;
        }

        public void Start(IExchange exchange, ExchangeWorkerConfiguration configuration) => Task.Run(async () =>
        {
            var limit = 1000;
            
            Exchange = exchange;

            Configuration = configuration;

            Logger = LoggerFactory.CreateLogger($"Historian.{exchange.Name}.Worker");

            using (Logger.BeginExchangeScope(exchange.Name))
            {
                await Ping(true);

                Heartbeat();

            }

            TradeWorker.Start(this, limit);
            TradeAggregateWorker.Start(this, limit);

            if (Exchange.SupportedStatKeys != null)
            {
                TradeStatWorker.Start(this);
                TradeStatAggregateWorker.Start(this, limit);
            }
        });

        private async Task Ping(bool initial = false)
        {
            try
            {
                var ping = new Ping();

                var uri = new Uri(Exchange.GetHttpClient().ApiUrl);

                var reply = await ping.SendPingAsync(uri.Host);

                if (reply.Status == IPStatus.Success)
                {
                    // If the exchange is running web socket, and supports historical loads, ensure the historian does not produce gaps
                    if (!initial && !Online && Exchange.SupportsHistoricalLoad && Exchange.GetWebSocketClient() != null)
                    {
                        Logger.LogInformation($"Regained connectivity");

                        var symbols = await HistorianRepository.GetSymbols(Exchange.Name);

                        foreach (var historianSymbol in symbols)
                        {
                            var symbol = SymbolFactory.Get(historianSymbol.SymbolCode);

                            if (!symbol.Tradable)
                                continue;

                            using (Logger.BeginSymbolScope(symbol.Code))
                            {
                                var now = Epoch.Now;

                                var catchup = new HistorianTradeCatchup
                                {
                                    Exchange = Exchange.Name,
                                    SymbolCode = historianSymbol.SymbolCode,
                                    TradeFilter = historianSymbol.TradeFilter,
                                    EpochTo = now,
                                    CurrentTradeFilter = historianSymbol.TradeFilter,
                                    Priority = 1
                                };

                                Logger.LogWarning($"Adding trade catch up job with filter '{historianSymbol.TradeFilter}'");

                                await HistorianRepository.AddTradeCatchup(catchup);
                            }
                        }
                    }

                    Online = true;
                }
                else
                {
                    Logger.LogWarning($"Lost connectivity: {reply.Status}");

                    Online = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, $"Unexpected error occurred when pinging");

                Online = false;
            }
        }

        private void Heartbeat() => Task.Run(async () =>
        {
            while (true)
            {
                await Ping();

                await Task.Delay(3000);
            }
        });
    }
}
