namespace CryptoCurrency.Core.Currency
{
    public class VeChain : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.VET;
        
        public string Label => "VeChain";

        public string Symbol => "VET";
    }
}
