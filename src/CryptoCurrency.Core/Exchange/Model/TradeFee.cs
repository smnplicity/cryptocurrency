using CryptoCurrency.Core.Currency;

namespace CryptoCurrency.Core.Exchange.Model
{
    public class TradeFee
    {
        public CurrencyCodeEnum CurrencyCode { get; set; }

        public decimal Maker { get; set; }

        public decimal Taker { get; set; }
    }
}
