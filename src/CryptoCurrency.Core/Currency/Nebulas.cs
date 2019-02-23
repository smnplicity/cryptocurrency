namespace CryptoCurrency.Core.Currency
{
    public class Nebulas : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.NAS;
        
        public string Label => "Nebulas";

        public string Symbol => "NAS";
    }
}
