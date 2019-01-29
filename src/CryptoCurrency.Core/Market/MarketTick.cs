using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Market
{
    public class MarketTick
    {
        public ExchangeEnum Exchange { get; set; }

        public SymbolCodeEnum SymbolCode { get; set; }

        public Epoch Epoch { get; set; }

        public double BuyPrice { get; set; }

        public double SellPrice { get; set; }

        public double LastPrice { get; set; }
    }
}
