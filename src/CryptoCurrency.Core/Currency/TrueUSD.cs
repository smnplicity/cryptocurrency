namespace CryptoCurrency.Core.Currency
{
    public class TrueUSD : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.TUSD;
        
        public string Label => "TrueUSD";

        public string Symbol => "TUSD";
    }
}
