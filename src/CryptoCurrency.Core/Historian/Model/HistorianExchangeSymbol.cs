using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Historian.Model
{
    public class HistorianExchangeSymbol
    {
        public ExchangeEnum Exchange { get; set; }

        public SymbolCodeEnum SymbolCode { get; set; }

        public string TradeFilter { get; set; }

        public long? LastTradeId { get; set; }

        public long? LastTradeStatId { get; set; }
    }
}
