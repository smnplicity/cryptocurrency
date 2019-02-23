namespace CryptoCurrency.Core.Currency
{
    public class Aeternity : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.AE;
        
        public string Label => "Aeternity";

        public string Symbol => "AE";
    }
}
