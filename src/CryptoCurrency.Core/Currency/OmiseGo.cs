namespace CryptoCurrency.Core.Currency
{
    public class OmiseGo : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.OMG;
        
        public string Label => "OmiseGo";

        public string Symbol => "OMG";
    }
}
