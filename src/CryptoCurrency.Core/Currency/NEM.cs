namespace CryptoCurrency.Core.Currency
{
    public class NEM : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.XEM;
        
        public string Label => "NEM";

        public string Symbol => "XEM";
    }
}
