using System.Collections.Generic;

using CryptoCurrency.Core.Market;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Exchange.Model
{
    public class TradeResult
    {
        public ExchangeEnum Exchange { get; set; }

        public SymbolCodeEnum SymbolCode { get; set; }

        public ICollection<MarketTrade> Trades { get; set; }

        public string Filter { get; set; }
    }
}
