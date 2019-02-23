using System.Collections.Generic;

using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.HistorianService.Worker
{
    public class ExchangeWorkerConfiguration
    {
        public ICollection<SymbolCodeEnum> Symbol { get; set; }

        public bool UseWebSocket { get; set; }
    }
}
