namespace CryptoCurrency.Core.Currency
{
    public class Ripple : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.XRP;
        
        public string Label => "Ripple";

        public string Symbol => "XRP";
    }
}