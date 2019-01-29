using CryptoCurrency.Core.Currency;

namespace CryptoCurrency.Core.Exchange.Model
{
    public class AccountBalance
    {
        public CurrencyCodeEnum CurrencyCode { get; set; }

        public double Balance { get; set; }

        public double PendingFunds { get; set; }
    }
}
