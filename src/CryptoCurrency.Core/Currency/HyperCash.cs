namespace CryptoCurrency.Core.Currency
{
    public class HyperCash : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.HC;
        
        public string Label => "HyperCash";

        public string Symbol => "HC";
    }
}
