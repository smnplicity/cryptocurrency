namespace CryptoCurrency.Core.Currency
{
    public class Walton : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.WTC;
        
        public string Label => "Walton";

        public string Symbol => "WTC";
    }
}
