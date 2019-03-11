using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Market
{
    public class MarketTradeStat
    {
        public long TradeStatId { get; set; }

        public ExchangeEnum Exchange { get; set; }

        public SymbolCodeEnum SymbolCode { get; set; }

        public Epoch Epoch { get; set; }

        public ExchangeStatsKeyEnum StatKey { get; set; }

        public decimal Value { get; set; }
    }
}
