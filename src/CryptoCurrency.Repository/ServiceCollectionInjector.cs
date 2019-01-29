using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Historian;
using CryptoCurrency.Core.Interval;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.Market;
using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.StorageTransaction;

using CryptoCurrency.Repository.Edm.Historian;

namespace CryptoCurrency.Repository
{
    public static class ServiceCollectionInjector
    {
        public static IServiceCollection AddRepositories(
            this IServiceCollection serviceCollection,
            string historianConnectionString,
            string loggingConnectionString)
        {
            serviceCollection
                .Configure<DbContextConfigurationOptions>(options =>
                {
                    options.HistorianConnectionString = historianConnectionString;
                    options.LoggingConnectionString = loggingConnectionString;
                })
                .AddSingleton<IDesignTimeDbContextFactory<HistorianDbContext>, HistorianDesignTimeDbContextFactory>()
                .AddSingleton<IStorageTransactionFactory<HistorianDbContext>, HistorianStorageTransactionFactory>()
                .AddSingleton<IHistorianRepository, HistorianRepository>()
                .AddSingleton<IExchangeRepository, ExchangeRepository>()
                .AddSingleton<IIntervalRepository, IntervalRepository>()
                .AddSingleton<ICurrencyRepository, CurrencyRepository>()
                .AddSingleton<ISymbolRepository, SymbolRepository>()
                .AddSingleton<IMarketRepository, MarketRepository>()
                .AddSingleton<IOrderSideRepository, OrderSideRepository>();
            
            return serviceCollection;
        }
    }
}