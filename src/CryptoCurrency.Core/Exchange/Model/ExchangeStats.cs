using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Exchange.Model
{
    public class ExchangeStats
    {
        public Epoch Epoch { get; set; }

        public ExchangeEnum Exchange { get; set; }

        public SymbolCodeEnum SymbolCode { get; set; }

        public ExchangeStatsKeyEnum StatKey { get; set; }

        public decimal Value { get; set; }
    }
}
