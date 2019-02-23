namespace CryptoCurrency.Core.Currency
{
    public class AppCoins : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.APPC;
        
        public string Label => "AppCoins";

        public string Symbol => "APPC";
    }
}
