namespace CryptoCurrency.Core.Currency
{
    public class USDC : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.USDC;
        
        public string Label => "USDC";

        public string Symbol => "USDC";
    }
}
