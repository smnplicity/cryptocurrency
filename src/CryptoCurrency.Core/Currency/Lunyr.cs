namespace CryptoCurrency.Core.Currency
{
    public class Lunyr : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.LUN;
        
        public string Label => "Lunyr";

        public string Symbol => "LUN";
    }
}
