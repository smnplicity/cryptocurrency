using Microsoft.Extensions.DependencyInjection;

using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Interval;
using CryptoCurrency.Core.Interval.Group;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.Exchange;

namespace CryptoCurrency.Core
{
    public static class ServiceCollectionInjector
    {
        public static IServiceCollection AddFactories(this IServiceCollection serviceCollection)
        {
            // Interval factory
            serviceCollection
                .AddSingleton<IIntervalGroup, Minute>()
                .AddSingleton<IIntervalGroup, Hour>()
                .AddSingleton<IIntervalGroup, Day>()
                .AddSingleton<IIntervalGroup, Week>()
                .AddSingleton<IIntervalGroup, Month>()
                .AddSingleton<IIntervalFactory, IntervalFactory>();

            // Currency factory
            serviceCollection
                .AddSingleton<ICurrency, Bitcoin>()
                .AddSingleton<ICurrency, Litecoin>()
                .AddSingleton<ICurrency, Ethereum>()
                .AddSingleton<ICurrency, EthereumClassic>()
                .AddSingleton<ICurrency, Ripple>()
                .AddSingleton<ICurrency, Aud>()
                .AddSingleton<ICurrency, Eur>()
                .AddSingleton<ICurrency, Usd>()
                .AddSingleton<ICurrency, Iota>()
                .AddSingleton<ICurrency, Neo>()
                .AddSingleton<ICurrency, Dash>()
                .AddSingleton<ICurrency, Tether>()
                .AddSingleton<ICurrency, StellarLumens>()
                .AddSingleton<ICurrencyFactory, CurrencyFactory>();

            // Symbol factory
            serviceCollection.AddSingleton<ISymbolFactory, SymbolFactory>();

            serviceCollection.AddSingleton<IExchangeTradeStatProvider, ExchangeTradeStatProvider>();

            // Order Side factory
            serviceCollection.AddSingleton<IOrderSideFactory, OrderSideFactory>();

            return serviceCollection;
        }
    }
}