namespace CryptoCurrency.Core.Currency
{
    public class ZenCash : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.ZEN;
        
        public string Label => "ZenCash";

        public string Symbol => "ZEN";
    }
}
