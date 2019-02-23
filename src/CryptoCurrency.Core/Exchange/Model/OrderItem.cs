using System;

using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.OrderState;
using CryptoCurrency.Core.OrderType;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Exchange.Model
{
    public class OrderItem
    {
        public ExchangeEnum Exchange { get; set; }

        public SymbolCodeEnum SymbolCode { get; set; }

        public string Id { get; set; }

        public OrderSideEnum Side { get; set; }

        public OrderTypeEnum Type { get; set; }

        public double Price { get; set; }

        public double AvgPrice { get; set; }

        public OrderStateEnum State { get; set; }

        public double Volume { get; set; }

        public double RemainingVolume { get; set; }

        public CurrencyCodeEnum FeeCurrencyCode { get; set; }

        public double Fee { get; set; }

        public Epoch OrderEpoch { get; set; }
    }
}
