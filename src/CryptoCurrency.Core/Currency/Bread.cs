namespace CryptoCurrency.Core.Currency
{
    public class Bread : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.BRD;
        
        public string Label => "Bread";

        public string Symbol => "BRD";
    }
}
