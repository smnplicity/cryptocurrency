using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Market
{
    public class Ohlc
    {
        public ExchangeEnum Exchange { get; set; }

        public SymbolCodeEnum Symbol { get; set; }

        public string IntervalKey { get; set; }

        public Epoch Epoch { get; set; }

        public decimal Open { get; set; }

        public Epoch OpenEpoch { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public Epoch CloseEpoch { get; set; }
    }
}
