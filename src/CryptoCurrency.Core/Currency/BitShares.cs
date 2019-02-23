namespace CryptoCurrency.Core.Currency
{
    public class BitShares : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.BTS;
        
        public string Label => "BitShares";

        public string Symbol => "BTS";
    }
}
