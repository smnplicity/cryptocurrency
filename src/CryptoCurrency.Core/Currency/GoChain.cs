namespace CryptoCurrency.Core.Currency
{
    public class GoChain : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.GO;
        
        public string Label => "GoChain";

        public string Symbol => "GO";
    }
}
