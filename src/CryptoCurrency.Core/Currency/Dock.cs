namespace CryptoCurrency.Core.Currency
{
    public class Dock : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.DOCK;
        
        public string Label => "Dock";

        public string Symbol => "DOCK";
    }
}
