namespace CryptoCurrency.Core.Currency
{
    public class Nano : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.NANO;
        
        public string Label => "Nano";

        public string Symbol => "NANO";
    }
}
