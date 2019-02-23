namespace CryptoCurrency.Core.Currency
{
    public class Wanchain : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.WAN;
        
        public string Label => "Wanchain";

        public string Symbol => "WAN";
    }
}
