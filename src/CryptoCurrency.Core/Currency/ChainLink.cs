namespace CryptoCurrency.Core.Currency
{
    public class ChainLink : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.LINK;
        
        public string Label => "ChainLink";

        public string Symbol => "LINK";
    }
}
