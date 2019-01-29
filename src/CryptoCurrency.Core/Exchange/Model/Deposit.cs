namespace CryptoCurrency.Core.Exchange.Model
{
    public class Deposit
    {
        public string CurrencyCode { get; set; }

        public double Amount { get; set; }

        public string TransactionId { get; set; }

        public DepositStateEnum State { get; set; }
    }
}
