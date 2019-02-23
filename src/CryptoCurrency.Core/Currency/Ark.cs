namespace CryptoCurrency.Core.Currency
{
    public class Ark : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.ARK;
        
        public string Label => "Ark";

        public string Symbol => "ARK";
    }
}
