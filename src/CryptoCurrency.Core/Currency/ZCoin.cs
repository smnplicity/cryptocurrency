namespace CryptoCurrency.Core.Currency
{
    public class ZCoin : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.XZC;
        
        public string Label => "ZCoin";

        public string Symbol => "XZC";
    }
}
