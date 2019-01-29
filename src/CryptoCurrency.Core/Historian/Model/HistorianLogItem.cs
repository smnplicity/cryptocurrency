using System;

using Microsoft.Extensions.Logging;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Historian.Model
{
    public class HistorianLogItem
    {
        public Epoch Epoch { get; set; }

        public LogLevel LogLevel { get; set; }

        public string Category { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public ExchangeEnum? Exchange { get; set; }

        public SymbolCodeEnum? SymbolCode { get; set; }

        public string Protocol { get; set; }

        public string Worker { get; set; }
    }
}
