namespace CryptoCurrency.Core.Currency
{
    public class Selfkey : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.KEY;
        
        public string Label => "Selfkey";

        public string Symbol => "KEY";
    }
}
