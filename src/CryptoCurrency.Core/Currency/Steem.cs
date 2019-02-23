namespace CryptoCurrency.Core.Currency
{
    public class Steem : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.STEEM;
        
        public string Label => "Steem";

        public string Symbol => "STEEM";
    }
}
