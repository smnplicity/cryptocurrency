namespace CryptoCurrency.Core.Currency
{
    public class NAVCoin : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.NAV;
        
        public string Label => "NAVCoin";

        public string Symbol => "NAV";
    }
}
