namespace CryptoCurrency.Core.Currency
{
    public class Eur : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.EUR;
        
        public string Label => "Euro";

        public string Symbol => "€";
    }
}