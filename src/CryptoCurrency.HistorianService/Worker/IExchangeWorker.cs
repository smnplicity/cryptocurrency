using Microsoft.Extensions.Logging;

using CryptoCurrency.Core.Exchange;

namespace CryptoCurrency.HistorianService.Worker
{
    public interface IExchangeWorker
    {
        ILogger Logger { get; set; }

        IExchange Exchange { get; }

        ExchangeWorkerConfiguration Configuration { get; set; }

        bool Online { get; }

        void Start(IExchange exchange, ExchangeWorkerConfiguration configuration);
    }
}