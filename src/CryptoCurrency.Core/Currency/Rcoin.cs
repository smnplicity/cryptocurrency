namespace CryptoCurrency.Core.Currency
{
    public class Rcoin : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.RCN;
        
        public string Label => "Rcoin";

        public string Symbol => "RCN";
    }
}
