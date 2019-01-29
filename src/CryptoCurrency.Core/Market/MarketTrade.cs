using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Market
{
    public class MarketTrade
    {
        public ExchangeEnum Exchange { get; set; }

        public SymbolCodeEnum SymbolCode { get; set; }

        public Epoch Epoch { get; set; }

        public long TradeId { get; set; }

        public double Price { get; set; }

        public double Volume { get; set; }

        public OrderSideEnum? Side { get; set; }

        public string SourceTradeId { get; set; }
    }
}
