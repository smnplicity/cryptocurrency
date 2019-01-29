namespace CryptoCurrency.Core.Currency
{
    public class Usd : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.USD;
        
        public string Label => "US dollars";

        public string Symbol => "$";
    }
}