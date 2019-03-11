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

        public decimal Price { get; set; }

        public decimal AvgPrice { get; set; }

        public OrderStateEnum State { get; set; }

        public decimal Volume { get; set; }

        public decimal RemainingVolume { get; set; }

        public CurrencyCodeEnum FeeCurrencyCode { get; set; }

        public decimal Fee { get; set; }

        public Epoch OrderEpoch { get; set; }
    }
}
