namespace CryptoCurrency.Core.Currency
{
    public class CoinDash : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.CDT;
        
        public string Label => "CoinDash";

        public string Symbol => "CDT";
    }
}
