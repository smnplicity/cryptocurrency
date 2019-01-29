using CryptoCurrency.Core.Currency;

namespace CryptoCurrency.Core.Exchange.Model
{
    public class ExchangeCurrency
    {
        public CurrencyCodeEnum CurrencyCode { get; set; }

        public int Precision { get; set; }

        public string AltCurrencyCode { get; set; }
    }
}
