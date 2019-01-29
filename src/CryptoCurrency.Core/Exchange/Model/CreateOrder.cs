using System;

using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.OrderState;
using CryptoCurrency.Core.OrderType;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Exchange.Model
{
    public class CreateOrder
    {
        public SymbolCodeEnum SymbolCode { get; set; }

        public DateTime OrderTime { get; set; }

        public string Id { get; set; }

        public OrderStateEnum State { get; set; }

        public OrderTypeEnum Type { get; set; }

        public OrderSideEnum Side { get; set; }

        public double Price { get; set; }

        public double Volume { get; set; }
    }
}
