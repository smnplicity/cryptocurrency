namespace CryptoCurrency.Core.Currency
{
    public class Civic : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.CVC;
        
        public string Label => "Civic";

        public string Symbol => "CVC";
    }
}
