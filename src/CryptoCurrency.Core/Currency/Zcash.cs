namespace CryptoCurrency.Core.Currency
{
    public class Zcash : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.ZEC;
        
        public string Label => "Zcash";

        public string Symbol => "ZEC";
    }
}
