using CryptoCurrency.Core.Currency;

namespace CryptoCurrency.Core.Exchange.Model
{
    public class TradeFee
    {
        public CurrencyCodeEnum CurrencyCode { get; set; }

        public double Maker { get; set; }

        public double Taker { get; set; }
    }
}
