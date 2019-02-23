namespace CryptoCurrency.Core.Currency
{
    public class Poet : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.POE;
        
        public string Label => "Poet";

        public string Symbol => "POE";
    }
}
