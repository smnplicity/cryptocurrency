using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Market;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.StorageTransaction;
using CryptoCurrency.Repository.Edm.Historian;

namespace CryptoCurrency.HistorianService.Provider
{
    public class ExchangeTradeProvider : IExchangeTradeProvider
    {
        private IStorageTransactionFactory<HistorianDbContext> StorageTransactionFactory { get; set; }

        private IMarketRepository MarketRepository { get; set; }

        public ExchangeTradeProvider(
            IStorageTransactionFactory<HistorianDbContext> storageTransactionFactory,
            IMarketRepository marketRepository)
        {
            StorageTransactionFactory = storageTransactionFactory;
            MarketRepository = marketRepository;
        }
        
        public async Task<TradeResult> ReceiveTradesHttp(IStorageTransaction transaction, ILogger logger, ExchangeEnum exchange, ISymbol symbol, IExchangeHttpClient httpClient, int limit, string lastTradeFilter)
        {
            logger.LogInformation($"Requesting trades from filter '{lastTradeFilter}'");

            var response = await httpClient.GetTrades(symbol, limit, lastTradeFilter);

            var tradeResult = response.Data;

            if (response.StatusCode != WrappedResponseStatusCode.Ok)
            {
                var errorCode = !string.IsNullOrEmpty(response.ErrorCode) ? $"Error Code: {response.ErrorCode} Message: " : "";

                logger.LogWarning($"Unable to get trades: {errorCode}{response.ErrorMessage}");

                return null;
            }

            if (tradeResult.Trades.Count > 0)
                await AddTrades(transaction, logger, tradeResult);

            return tradeResult;
        }

        public async Task AddTrades(IStorageTransaction transaction, ILogger logger, TradeResult tradeResult)
        {
            try
            {
                var trades = tradeResult.Trades
                        .Select(trade => new MarketTrade
                        {
                            Epoch = trade.Epoch,
                            Exchange = tradeResult.Exchange,
                            SymbolCode = tradeResult.SymbolCode,
                            Side = trade.Side.HasValue ? trade.Side : null,
                            Price = trade.Price,
                            Volume = trade.Volume,
                            SourceTradeId = trade.SourceTradeId
                        }).OrderBy(t => t.Epoch.TimestampMilliseconds).ToList();

                if (trades.Count > 0)
                    await MarketRepository.SaveTrades(transaction, trades);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Unable to save trades into the repository.");
            }
        }
    }
}
