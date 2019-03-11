using System.Collections.Generic;

using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Market
{
    public class MarketTickAverage
    {
        public SymbolCodeEnum SymbolCode { get; set; }

        public Epoch Epoch { get; set; }

        public decimal BuyPrice { get; set; }

        public decimal SellPrice { get; set; }

        public decimal LastPrice { get; set; }

        public ICollection<ExchangeEnum> Exchanges { get; set; }
    }
}
