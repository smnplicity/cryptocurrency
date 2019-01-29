using System;
using System.Collections.Generic;

using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.OrderState;
using CryptoCurrency.Core.OrderType;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Exchange.Model
{
    public class OrderBookItem
    {
        public long Id { get; set; }

        public OrderSideEnum Side { get; set; }

        public OrderTypeEnum Type { get; set; }

        public double Price { get; set; }

        public double AvgPrice { get; set; }

        public OrderStateEnum State { get; set; }

        public DateTime OrderTime { get; set; }

        public double Volume { get; set; }

        public double RemainingVolume { get; set; }
    }

    public class OrderBook
    {
        public ExchangeEnum Exchange { get; set; }

        public SymbolCodeEnum SymbolCode { get; set; }

        public ICollection<OrderBookItem> Ask { get; set; }

        public ICollection<OrderBookItem> Bid { get; set; }
    }
}
