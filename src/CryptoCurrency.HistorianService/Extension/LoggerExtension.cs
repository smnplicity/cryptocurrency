using System;

using Microsoft.Extensions.Logging;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.HistorianService.Extension
{
    public static class LoggerExtension
    {
        public static IDisposable BeginExchangeScope(this ILogger logger, ExchangeEnum exchange)
        {
            return logger.BeginScope(new { Exchange = exchange });
        }

        public static IDisposable BeginWorkerScope(this ILogger logger, string worker)
        {
            return logger.BeginScope(new { Worker = worker });
        }

        public static IDisposable BeginSymbolScope(this ILogger logger, SymbolCodeEnum symbolCode)
        {
            return logger.BeginScope(new { SymbolCode = symbolCode });
        }

        public static IDisposable BeginExchangeStatsScope(this ILogger logger, ExchangeStatsKeyEnum statsKey)
        {
            return logger.BeginScope(new { StatsKey = statsKey });
        }

        public static IDisposable BeginProtocolScope(this ILogger logger, string protocol)
        {
            return logger.BeginScope(new { Protocol = protocol });
        }
    }
}