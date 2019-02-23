namespace CryptoCurrency.Core.Currency
{
    public class StableUSD : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.USDS;
        
        public string Label => "StableUSD";

        public string Symbol => "USDS";
    }
}
