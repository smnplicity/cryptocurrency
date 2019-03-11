namespace CryptoCurrency.Core.Exchange.Model
{
    public class Deposit
    {
        public string CurrencyCode { get; set; }

        public decimal Amount { get; set; }

        public string TransactionId { get; set; }

        public DepositStateEnum State { get; set; }
    }
}
