using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Interval;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Historian;
using CryptoCurrency.Core.StorageTransaction;

using CryptoCurrency.Repository.Edm.Historian;

namespace CryptoCurrency.HistorianService
{
    public interface IRepositoryBootstrapper
    {
        Task<bool> Run(ILogger logger);
    }

    public class RepositoryBootstrapper : IRepositoryBootstrapper
    {
        private ILoggerFactory LoggerFactory { get; set; }

        private IExchangeFactory ExchangeFactory { get; set; }

        private IIntervalFactory IntervalFactory { get; set; }

        private ICurrencyFactory CurrencyFactory { get; set; }

        private ISymbolFactory SymbolFactory { get; set; }

        private IOrderSideFactory OrderSideFactory { get; set; }

        private IStorageTransactionFactory<HistorianDbContext> StorageTransactionFactory { get; set; }

        private IHistorianRepository HistorianRepository { get; set; }

        private IExchangeRepository ExchangeRepository { get; set; }

        private IIntervalRepository IntervalRepository { get; set; }

        private ICurrencyRepository CurrencyRepository { get; set; }

        private ISymbolRepository SymbolRepository { get; set; }

        private IOrderSideRepository OrderSideRepository { get; set; }

        public RepositoryBootstrapper(
            IExchangeFactory exchangeFactory,
            IIntervalFactory intervalFactory,
            ICurrencyFactory currencyFactory, 
            ISymbolFactory symbolFactory,
            IOrderSideFactory orderSideFactory,
            IStorageTransactionFactory<HistorianDbContext> storageTransactionFactory,
            IHistorianRepository historianRepository,
            IExchangeRepository exchangeRepository,
            IIntervalRepository intervalRepository,
            ICurrencyRepository currencyRepository,
            ISymbolRepository symbolRepository,
            IOrderSideRepository orderSideRepository)
        {
            ExchangeFactory = exchangeFactory;
            IntervalFactory = intervalFactory;
            CurrencyFactory = currencyFactory;
            SymbolFactory = symbolFactory;
            OrderSideFactory = orderSideFactory;
            StorageTransactionFactory = storageTransactionFactory;
            HistorianRepository = historianRepository;
            ExchangeRepository = exchangeRepository;
            IntervalRepository = intervalRepository;
            CurrencyRepository = currencyRepository;
            SymbolRepository = symbolRepository;
            OrderSideRepository = orderSideRepository;
        }

        public async Task<bool> Run(ILogger logger)
        {
            logger.LogInformation("Running repository bootstrapper");

            try
            {
                foreach (var currency in CurrencyFactory.List())
                    await CurrencyRepository.Add(currency);

                foreach (var symbol in SymbolFactory.List())
                    await SymbolRepository.Add(symbol);

                foreach(var group in IntervalFactory.ListGroups())
                {
                    foreach(var ik in IntervalFactory.ListIntervalKeys(group.IntervalGroup))
                    {
                        await IntervalRepository.Add(ik);                        
                    }
                }

                foreach (var exchange in ExchangeFactory.List())
                {
                    await ExchangeRepository.Add(exchange);

                    var httpClient = exchange.GetHttpClient();

                    foreach (var symbolCode in exchange.Symbol)
                    {
                        var symbol = SymbolFactory.Get(symbolCode);

                        await SymbolRepository.Add(symbol);

                        await ExchangeRepository.AddSymbol(exchange.Name, symbolCode);

                        var tradeFilter = await HistorianRepository.GetTradeFilter(exchange.Name, symbolCode);

                        if (tradeFilter == null)
                        {
                            using (var transaction = await StorageTransactionFactory.Begin())
                            {
                                await HistorianRepository.SetTradeFilter(transaction, exchange.Name, symbolCode, httpClient.InitialTradeFilter);
                                
                                await transaction.Commit();
                            }
                        }
                    }
                }

                foreach (var orderSide in OrderSideFactory.List())
                    await OrderSideRepository.Add(orderSide);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Unable to run repository bootstrapper");

                return false;
            }
        }
    }
}
