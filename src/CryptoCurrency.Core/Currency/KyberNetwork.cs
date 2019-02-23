namespace CryptoCurrency.Core.Currency
{
    public class KyberNetwork : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.KNC;
        
        public string Label => "KyberNetwork";

        public string Symbol => "KNC";
    }
}
