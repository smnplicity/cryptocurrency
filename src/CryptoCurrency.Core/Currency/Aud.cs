namespace CryptoCurrency.Core.Currency
{
    public class Aud : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.AUD;
        
        public string Label => "Australian dollars";

        public string Symbol => "$";
    }
}
