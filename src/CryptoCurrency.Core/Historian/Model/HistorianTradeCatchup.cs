using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Historian.Model
{
    public class HistorianTradeCatchup
    {
        public ExchangeEnum Exchange { get; set; }

        public SymbolCodeEnum SymbolCode { get; set; }

        public Epoch EpochTo { get; set; }

        public string TradeFilter { get; set; }
        
        public string CurrentTradeFilter { get; set; }

        public int Priority { get; set; }
    }
}
