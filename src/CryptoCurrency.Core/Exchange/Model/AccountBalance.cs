using CryptoCurrency.Core.Currency;

namespace CryptoCurrency.Core.Exchange.Model
{
    public class AccountBalance
    {
        public CurrencyCodeEnum CurrencyCode { get; set; }

        public decimal Balance { get; set; }

        public decimal PendingFunds { get; set; }
    }
}
