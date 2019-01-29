using Microsoft.Extensions.DependencyInjection;

using CryptoCurrency.Core.Exchange;

namespace CryptoCurrency.ExchangeClient
{
    public static class ServiceCollectionInjector
    {
        public static IServiceCollection AddExchangeFactory(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IExchange, Kraken.Kraken>()
                .AddSingleton<IExchange, Bitfinex.Bitfinex>()
                .AddSingleton<IExchange, Binance.Binance>()
                .AddSingleton<IExchange, CoinbasePro.CoinbasePro>()
                .AddSingleton<IExchangeFactory, ExchangeFactory>();

            return serviceCollection;
        }
    }
}