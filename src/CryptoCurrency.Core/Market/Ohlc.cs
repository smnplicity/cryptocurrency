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

        public double Open { get; set; }

        public Epoch OpenEpoch { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Close { get; set; }

        public Epoch CloseEpoch { get; set; }
    }
}
