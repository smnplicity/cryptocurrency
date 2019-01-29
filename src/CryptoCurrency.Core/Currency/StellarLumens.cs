namespace CryptoCurrency.Core.Currency
{
    public class StellarLumens : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.XLM;
        
        public string Label => "Stellar Lumens";

        public string Symbol => "XLM";
    }
}