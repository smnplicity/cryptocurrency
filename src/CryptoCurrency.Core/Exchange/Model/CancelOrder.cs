using CryptoCurrency.Core.OrderState;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Exchange.Model
{
    public class CancelOrder
    {
        public ExchangeEnum Exchange { get; set; }

        public SymbolCodeEnum SymbolCode { get; set; }

        public Epoch Epoch { get; set; }

        public string Id { get; set; }

        public OrderStateEnum State { get; set; }
    }
}
