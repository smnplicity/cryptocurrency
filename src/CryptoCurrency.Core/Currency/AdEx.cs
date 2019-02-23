namespace CryptoCurrency.Core.Currency
{
    public class AdEx : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.ADX;
        
        public string Label => "AdEx";

        public string Symbol => "ADX";
    }
}
